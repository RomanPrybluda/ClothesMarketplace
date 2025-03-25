using DAL;

namespace Domain
{
    public class ProductSizeInitializer
    {
        private readonly ClothesMarketplaceDbContext _context;

        public ProductSizeInitializer(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public void InitializeProductSizes()
        {

            if (!_context.ProductSizes.Any())
            {
                var productSizes = new List<ProductSize>
                {
                    new ProductSize { Id = Guid.NewGuid(), Value = "Small" },
                    new ProductSize { Id = Guid.NewGuid(), Value = "Medium" },
                    new ProductSize { Id = Guid.NewGuid(), Value = "Large" },
                    new ProductSize { Id = Guid.NewGuid(), Value = "X-Large" },
                    new ProductSize { Id = Guid.NewGuid(), Value = "XX-Large" },
                    new ProductSize { Id = Guid.NewGuid(), Value = "One Size" }
                };

                _context.ProductSizes.AddRange(productSizes);
                _context.SaveChanges();
            }
        }
    }
}
