using DAL;

namespace Domain
{
    public class CategoryInitializer
    {
        private readonly ClothesMarketplaceDbContext _context;

        public CategoryInitializer(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public void InitializeCategories()
        {
            if (!_context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Id = Guid.NewGuid(), Name = "Clothes" },
                    new Category { Id = Guid.NewGuid(), Name = "Shoes" },
                    new Category { Id = Guid.NewGuid(), Name = "Bags" },
                    new Category { Id = Guid.NewGuid(), Name = "Accessories" },
                };

                _context.Categories.AddRange(categories);
                _context.SaveChanges();
            }
        }
    }
}