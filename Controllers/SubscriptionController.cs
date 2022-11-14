using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Span.Culturio.Api.Models.Subscription;
using Span.Culturio.Api.Services.Subscriptions;
using System.ComponentModel.DataAnnotations;

namespace Span.Culturio.Api.Controllers
{
    [Route("subscriptions")]
    [Tags("Subscriptions")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        /// <summary>
        /// Get user subscriptions
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<SubscriptionDto>> GetSubscription(int id)
        {
            var subscription = await _subscriptionService.GetAsync(id);

            if (subscription is null)
            {
                return BadRequest();
            }
            return Ok(subscription);
        }

        /// <summary>
        /// Create a subscription
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<string>> CreateSubscription([Required] CreateSubscriptionDto createSubscriptionDto)
        {
            var subscription = await _subscriptionService.CreateAsync(createSubscriptionDto);
            return Ok("Created subscription");
        }

        /// <summary>
        /// Track single visit
        /// </summary>
        [HttpPost("track-visit")]
        public async Task<ActionResult<string>> TrackVisit([Required] TrackVisitDto trackVisitDto)
        {
            string result = await _subscriptionService.TrackVisit(trackVisitDto);

            if (result == "Bad request")
            {
                return BadRequest("Bad request");
            }
            return Ok("Visit to culture object recorded");
        }

        /// <summary>
        /// Activate subscription
        /// </summary>
        [HttpPost("Activate subscription")]
        public async Task<ActionResult<string>> Activate([Required] ActivateDto activateDto)
        {
            string result = await _subscriptionService.Activate(activateDto);

            if (result == "Subsription already active")
            {
                return BadRequest("Subsription already active");
            }
            return Ok("Subcription activated");
        }
    }
}
