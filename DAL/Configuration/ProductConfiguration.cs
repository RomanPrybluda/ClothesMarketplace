using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(p => p.DollarPrice)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.ForWhom)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ForWhomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Images)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}