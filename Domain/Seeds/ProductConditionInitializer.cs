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
                    new ProductCondition { Id = Guid.Parse("29237B66-57D0-4BE9-9BE5-8724C3010AD6"), Name = "New" },
                    new ProductCondition { Id = Guid.Parse("DC094E7D-2187-4FD5-BFC8-8E6BA43D23AD"), Name = "Like New" },
                    new ProductCondition { Id = Guid.Parse("65066B06-B1C6-4CDC-93B2-C48EC85B728A"), Name = "Used" },
                    new ProductCondition { Id = Guid.Parse("32971903-985C-488F-85FA-8EEC94EC4E33"), Name = "Needs Repair" }
                };

                _context.ProductConditions.AddRange(productConditions);
                _context.SaveChanges();
            }
        }
    }
}
