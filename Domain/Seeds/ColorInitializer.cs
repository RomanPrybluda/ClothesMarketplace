using DAL;

namespace Domain
{
    public class ColorInitializer
    {
        private readonly ClothesMarketplaceDbContext _context;

        public ColorInitializer(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public void InitializeColors()
        {

            if (!_context.Colors.Any())
            {
                var colors = new List<Color>
                {
                    new Color { Id = Guid.NewGuid(), Name = "Red" },
                    new Color { Id = Guid.NewGuid(), Name = "Blue" },
                    new Color { Id = Guid.NewGuid(), Name = "Green" },
                    new Color { Id = Guid.NewGuid(), Name = "Yellow" },
                    new Color { Id = Guid.NewGuid(), Name = "Black" },
                    new Color { Id = Guid.NewGuid(), Name = "White" },
                    new Color { Id = Guid.NewGuid(), Name = "Pink" },
                    new Color { Id = Guid.NewGuid(), Name = "Purple" },
                    new Color { Id = Guid.NewGuid(), Name = "Orange" },
                    new Color { Id = Guid.NewGuid(), Name = "Brown" }
                };

                _context.Colors.AddRange(colors);
                _context.SaveChanges();
            }
        }
    }
}
