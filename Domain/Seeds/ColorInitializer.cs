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
                    new Color { Id = Guid.Parse("D441010C-DC5E-47D0-BE9F-1DF046FE25C0"), Name = "Red" },
                    new Color { Id = Guid.Parse("81329961-641B-4EC8-A417-299826251B48"), Name = "Blue" },
                    new Color { Id = Guid.Parse("3AE98DFC-7C16-4A22-9A93-69D0899AD4FC"), Name = "Green" },
                    new Color { Id = Guid.Parse("1DE5BF6A-D70B-45CE-820D-F58F6FED5CD2"), Name = "Yellow" },
                    new Color { Id = Guid.Parse("A8E212CA-C271-41A6-809E-0FB45F3D33A8"), Name = "Black" },
                    new Color { Id = Guid.Parse("2207B835-5D95-4869-B34F-2B05426C3AD6"), Name = "White" },
                    new Color { Id = Guid.Parse("07658533-B006-44E1-A667-87D0276C544D"), Name = "Pink" },
                    new Color { Id = Guid.Parse("A80E7596-6994-4232-8C7E-ADA9B3A98480"), Name = "Purple" },
                    new Color { Id = Guid.Parse("0E12BDCB-5338-4D10-B0B3-26F86A6B0F95"), Name = "Orange" },
                    new Color { Id = Guid.Parse("3881D4CB-6096-47C3-BD42-A65EC0C6C921"), Name = "Brown" }
                };

                _context.Colors.AddRange(colors);
                _context.SaveChanges();
            }
        }
    }
}
