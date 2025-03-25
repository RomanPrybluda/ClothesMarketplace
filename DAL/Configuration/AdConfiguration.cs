using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class AdConfiguration : IEntityTypeConfiguration<Ad>
    {
        public void Configure(EntityTypeBuilder<Ad> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.Description)
                .HasMaxLength(1000);

            builder.Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(a => a.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnUpdate();

            builder.Property(a => a.IsActive)
                .HasDefaultValue(true);

            builder.HasOne(a => a.Product)
                .WithOne(p => p.Ad)
                .HasForeignKey<Product>(p => p.AdId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
