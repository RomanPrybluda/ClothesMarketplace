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
                    new ProductSize { Id = Guid.Parse("D6C3E61D-AF7E-48B1-B8C2-1C86DED103DC"), Value = "Small" },
                    new ProductSize { Id = Guid.Parse("32455FC0-E34C-45B4-B0CB-ED608488A831"), Value = "Medium" },
                    new ProductSize { Id = Guid.Parse("B7D39B1D-CE5F-4A12-8679-A5790828C421"), Value = "Large" },
                    new ProductSize { Id = Guid.Parse("2D90D334-F644-4014-B845-1DB20C0A284F"), Value = "X-Large" },
                    new ProductSize { Id = Guid.Parse("4B094BB1-54AE-418E-92F2-505DA57D1E17"), Value = "XX-Large" },
                    new ProductSize { Id = Guid.Parse("EA14AD4F-F7F6-4770-8A1E-6E5F69B3CB53"), Value = "One Size" }
                };

                _context.ProductSizes.AddRange(productSizes);
                _context.SaveChanges();
            }
        }
    }
}
