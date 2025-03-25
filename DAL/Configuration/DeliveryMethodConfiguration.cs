using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Configurations
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder
                .HasKey(dm => dm.Id);

            builder
                .Property(c => c.Id)
                .HasDefaultValueSql("NEWID()");

            builder.Property(dm => dm.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(dm => dm.Ads)
                .WithOne(ad => ad.DeliveryMethod)
                .HasForeignKey(ad => ad.DeliveryMethodId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}