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
            
            builder.Entity<AppUser>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.FirstName).HasMaxLength(100);
                entity.Property(u => u.LastName).HasMaxLength(100);
            });

            builder.Entity<IdentityRole>(entity =>
            {
                builder.ApplyConfigurationsFromAssembly(typeof(ClothesMarketplaceDbContext).Assembly);
                base.OnModelCreating(builder);
                
                builder.Entity<AppUser>(entity =>
                {
                    entity.HasIndex(u => u.Email).IsUnique();
                    entity.Property(u => u.FirstName).HasMaxLength(100);
                    entity.Property(u => u.LastName).HasMaxLength(100);
                });

                builder.Entity<IdentityRole>(entity =>
                {
                    entity.HasData(
                        new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" },
                        new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Seller", NormalizedName = "SELLER" },
                        new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Buyer", NormalizedName = "BUYER" }
                    );
                });
            });
        }    
    }
}
