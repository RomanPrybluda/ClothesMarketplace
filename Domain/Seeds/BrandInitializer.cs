using DAL;

namespace Domain
{
    public class BrandInitializer
    {
        private readonly ClothesMarketplaceDbContext _context;

        public BrandInitializer(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public void InitializeBrands()
        {
            if (!_context.Brands.Any())
            {
                var brands = new List<Brand>
                {
                    new Brand { Id = Guid.NewGuid(), Name = "Nike" },
                    new Brand { Id = Guid.NewGuid(), Name = "Adidas" },
                    new Brand { Id = Guid.NewGuid(), Name = "Puma" },
                    new Brand { Id = Guid.NewGuid(), Name = "Reebok" },
                    new Brand { Id = Guid.NewGuid(), Name = "Gucci" },
                    new Brand { Id = Guid.NewGuid(), Name = "Prada" },
                    new Brand { Id = Guid.NewGuid(), Name = "Louis Vuitton" },
                    new Brand { Id = Guid.NewGuid(), Name = "Zara" },
                    new Brand { Id = Guid.NewGuid(), Name = "H&M" },
                    new Brand { Id = Guid.NewGuid(), Name = "Levi's" }
                };

                _context.Brands.AddRange(brands);
                _context.SaveChanges();
            }
        }
    }
}
