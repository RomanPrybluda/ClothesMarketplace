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
                    new ForWhom { Id = Guid.Parse("9196E79E-3537-4504-8DC2-16644606F087"), Name = "Men" },
                    new ForWhom { Id = Guid.Parse("4DD38FE2-FA02-45D8-8864-350D458A613D"), Name = "Women" },
                    new ForWhom { Id = Guid.Parse("E54A839F-E472-4543-8F41-4DE52747F414"), Name = "Kids" },
                    new ForWhom { Id = Guid.Parse("1C6B6970-3947-4856-8352-06856DD20D81"), Name = "Unisex" }
                };

                _context.ForWhoms.AddRange(forWhomCategories);
                _context.SaveChanges();
            }
        }
    }
}
