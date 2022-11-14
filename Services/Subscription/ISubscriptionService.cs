using Span.Culturio.Api.Models.Subscription;

namespace Span.Culturio.Api.Services.Subscriptions
{
    public interface ISubscriptionService
    {
        Task<string> TrackVisit(TrackVisitDto trackVisitDto);
        Task<string> Activate(ActivateDto activateDto);
        Task<SubscriptionDto> CreateAsync(CreateSubscriptionDto createSubscriptionDto);
        Task<SubscriptionDto> GetAsync(int userId);
    }
}
