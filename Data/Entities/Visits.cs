using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Span.Culturio.Api.Data.Entities
{
    public class Visits
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public int CultureObjectId { get; set; }
        public int AvailableVisits { get; set; }

        public virtual Subscription Subscription { get; set; }
    }

    public class VisitsConfiguration : IEntityTypeConfiguration<Visits>
    {
        public void Configure(EntityTypeBuilder<Visits> builder)
        {
            builder.ToTable("Visits");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Subscription).WithMany(x => x.Visits).HasForeignKey(x => x.SubscriptionId);
        }
    }
}
