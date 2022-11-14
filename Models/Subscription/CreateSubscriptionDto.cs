namespace Span.Culturio.Api.Models.Subscription
{
    public class CreateSubscriptionDto
    {
        public int UserId { get; set; }
        public int PackageId { get; set; }
        public string Name { get; set; }
    }
}
