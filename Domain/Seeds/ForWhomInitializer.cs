using DAL;

namespace Domain
{
    public class ForWhomInitializer
    {
        private readonly ClothesMarketplaceDbContext _context;

        public ForWhomInitializer(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public void InitializeForWhom()
        {
            if (!_context.ForWhoms.Any())
            {
                var forWhomCategories = new List<ForWhom>
                {
                    new ForWhom { Id = Guid.NewGuid(), Name = "Men" },
                    new ForWhom { Id = Guid.NewGuid(), Name = "Women" },
                    new ForWhom { Id = Guid.NewGuid(), Name = "Kids" },
                    new ForWhom { Id = Guid.NewGuid(), Name = "Unisex" }
                };

                _context.ForWhoms.AddRange(forWhomCategories);
                _context.SaveChanges();
            }
        }
    }
}
