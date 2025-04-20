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
                    new Category { Id = Guid.Parse("5E57F154-6F54-494D-B97A-FF25843EB849"), Name = "Clothes" },
                    new Category { Id = Guid.Parse("86F9211A-F56C-448D-A4BF-7E8A1B395231"), Name = "Shoes" },
                    new Category { Id = Guid.Parse("68C5D057-0902-4D19-8304-7F69510417AA"), Name = "Bags" },
                    new Category { Id = Guid.Parse("552A7F36-7126-45BC-A4D8-0B65B76700A7"), Name = "Accessories" },
                };

                _context.Categories.AddRange(categories);
                _context.SaveChanges();
            }
        }
    }
}