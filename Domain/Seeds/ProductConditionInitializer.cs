using DAL;

namespace Domain
{
    public class ProductConditionInitializer
    {
        private readonly ClothesMarketplaceDbContext _context;

        public ProductConditionInitializer(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public void InitializeProductConditions()
        {

            if (!_context.ProductConditions.Any())
            {
                var productConditions = new List<ProductCondition>
                {
                    new ProductCondition { Id = Guid.NewGuid(), Name = "New" },
                    new ProductCondition { Id = Guid.NewGuid(), Name = "Like New" },
                    new ProductCondition { Id = Guid.NewGuid(), Name = "Used" },
                    new ProductCondition { Id = Guid.NewGuid(), Name = "Needs Repair" }
                };

                _context.ProductConditions.AddRange(productConditions);
                _context.SaveChanges();
            }
        }
    }
}
