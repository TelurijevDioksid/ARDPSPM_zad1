using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Span.Culturio.Api.Data;
using Span.Culturio.Api.Data.Entities;
using Span.Culturio.Api.Models.Subscription;

namespace Span.Culturio.Api.Services.Subscriptions
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SubscriptionService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> Activate(ActivateDto activateDto)
        {
            var subsription = await _context.Subscriptions.FindAsync(activateDto.SubscriptionId);
            if (subsription is null)
            {
                return null;
            }
            if (subsription.State == "Active")
            {
                return "Subsription already active";
            }
            var days = (await _context.Packages.FindAsync(subsription.PackageId)).ValidDays;
            subsription.ActiveFrom = DateTime.Now;
            subsription.ActiveTo = DateTime.Now.AddDays(days);
            subsription.State = "Active";
            _context.Subscriptions.Update(subsription);
            await _context.SaveChangesAsync();

            return "Subscription activated";
        }

        public async Task<SubscriptionDto> CreateAsync(CreateSubscriptionDto createSubscriptionDto)
        {
            var subscription = _mapper.Map<Subscription>(createSubscriptionDto);
            var package = await _context.Packages.FindAsync(createSubscriptionDto.PackageId);
            var packageItems = await _context.PackageItems.Where(x => x.PackageId == subscription.PackageId).ToListAsync();

            subscription.ActiveFrom = null;
            subscription.ActiveTo = null;
            subscription.State = "Inactive";
            subscription.RecordedVisits = 0;

            await _context.AddAsync(subscription);
            await _context.SaveChangesAsync();

            int count = _context.Subscriptions.Count();
            int id = _context.Subscriptions.Skip(count - 1).Single().Id;

            packageItems.ForEach(async x => {
                var Visit = new Visits()
                {
                    Id = 0,
                    SubscriptionId = id,
                    CultureObjectId = x.CultureObjectId,
                    AvailableVisits = x.VisitsAvailable,
                };
                _context.Add(Visit);
                _context.SaveChanges();
            });

            var subscriptionDto = _mapper.Map<SubscriptionDto>(subscription);
            return subscriptionDto;
        }

        public async Task<SubscriptionDto> GetAsync(int userId)
        {
            var subscription = await _context.Subscriptions.FindAsync(userId);
            if (subscription is null)
            {
                return null;
            }
            var subscriptionDto = _mapper.Map<SubscriptionDto>(subscription);

            return subscriptionDto;
        }

        public async Task<string> TrackVisit(TrackVisitDto trackVisitDto)
        {
            var subscription = await _context.Subscriptions.FindAsync(trackVisitDto.SubscriptonId);

            if (subscription.State == "Inactive")
            {
                return "Bad request";
            }

            if (subscription is null)
            {
                return null;
            }

            var subscriptionDto = _mapper.Map<SubscriptionDto>(subscription);

            var package = await _context.Packages.FindAsync(subscriptionDto.PackageId);
            if (package is null)
            {
                return null;
            }

            var packageItems = await _context.PackageItems.ToListAsync();
            var cultureObjects = packageItems.Where(x => x.PackageId == subscriptionDto.PackageId).ToList();

            if (cultureObjects is null)
            {
                return null;
            }

            Boolean IsValid = false;
            cultureObjects.ForEach(x => {
                if (x.CultureObjectId == trackVisitDto.CultureObjectId)
                {
                    IsValid = true;
                }
            });

            var visit = _context.Visits.Where(x => x.CultureObjectId == trackVisitDto.CultureObjectId).Where(x => x.SubscriptionId == trackVisitDto.SubscriptonId).SingleOrDefault();

            if (IsValid && visit.AvailableVisits > 0 && visit is not null)
            {
                visit.AvailableVisits = visit.AvailableVisits - 1;
                _context.Visits.Update(visit);
                subscription.RecordedVisits = ++subscription.RecordedVisits;
                _context.Subscriptions.Update(subscription);
                await _context.SaveChangesAsync();
                return "Visit to culture object recorded";
            }
            else
            {
                return "Bad request";
            }
        }
    }
}
