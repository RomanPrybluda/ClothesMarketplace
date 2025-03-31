using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ClothesMarketplaceDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public ClothesMarketplaceDbContext(DbContextOptions options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<Ad> Ads { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        public DbSet<ForWhom> ForWhoms { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCondition> ProductConditions { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<ProductSize> ProductSizes { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ClothesMarketplaceDbContext).Assembly);
            base.OnModelCreating(builder);

             builder.Entity<AppUser>()
            .HasMany(u => u.PurchasedProducts)
            .WithOne()
            .HasForeignKey("BuyerId")
            .OnDelete(DeleteBehavior.Restrict);

             builder.Entity<AppUser>()
            .HasMany(u => u.SoldProducts)
            .WithOne()
            .HasForeignKey("SellerId")
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
