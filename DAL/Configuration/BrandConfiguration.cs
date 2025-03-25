using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder
               .HasKey(c => c.Id);

            builder
                .Property(c => c.Id)
                .HasDefaultValueSql("NEWID()");

            builder.HasMany(c => c.Products)
                .WithOne(p => p.Brand)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
