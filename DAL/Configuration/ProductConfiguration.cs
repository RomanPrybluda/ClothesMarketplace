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

            builder.Property(p => p.LikesCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.ForWhom)
                .WithMany(f => f.Products)
                .HasForeignKey(p => p.ForWhomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Color)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ColorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.ProductSize)
                .WithMany()
                .HasForeignKey(p => p.ProductSizeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.ProductCondition)
                .WithMany(pc => pc.Products)
                .HasForeignKey(p => p.ProductConditionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.ProductDetails)
                .WithOne(d => d.Product)
                .HasForeignKey<Product>(p => p.ProductDetailsId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.Seller)
                .WithMany(u => u.SoldProducts)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Buyer)
                .WithMany(u => u.PurchasedProducts)
                .HasForeignKey(p => p.BuyerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(p => p.Images)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(p => p.FavoritedByUsers)
                .WithMany(u => u.FavoriteProducts)
                .UsingEntity(j => j.ToTable("FavoriteProducts"));
        }
    }
}
