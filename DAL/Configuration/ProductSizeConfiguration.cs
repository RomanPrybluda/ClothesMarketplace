using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class ProductSizeConfiguration : IEntityTypeConfiguration<ProductSize>
    {
        public void Configure(EntityTypeBuilder<ProductSize> builder)
        {
            builder.HasKey(ps => ps.Id);

            builder.Property(ps => ps.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(ps => ps.Value)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(ps => ps.Products)
                .WithOne(p => p.ProductSize)
                .HasForeignKey(p => p.ProductSizeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
