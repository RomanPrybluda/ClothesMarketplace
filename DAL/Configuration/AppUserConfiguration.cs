using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .HasMaxLength(100);

        builder.Property(u => u.LastName)
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasMany(u => u.SoldProducts)
            .WithOne(p => p.Seller)
            .HasForeignKey(p => p.SellerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.PurchasedProducts)
            .WithOne(p => p.Buyer)
            .HasForeignKey(p => p.BuyerId);

        builder.HasMany(u => u.FavoriteProducts)
            .WithMany(p => p.FavoritedByUsers)
            .UsingEntity(j => j.ToTable("FavoriteProducts"));
    }
}
