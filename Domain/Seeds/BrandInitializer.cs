using DAL;

namespace Domain
{
    public class BrandInitializer
    {
        private readonly ClothesMarketplaceDbContext _context;

        public BrandInitializer(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public void InitializeBrands()
        {
            if (!_context.Brands.Any())
            {
                var brands = new List<Brand>
                {
                    new Brand { Id = Guid.Parse("0DB0AF0E-F3E4-481D-AA9B-E6BECC448B52"), Name = "Nike" },
                    new Brand { Id = Guid.Parse("0A28845F-BACD-4893-9D13-C2754B5E1DD6"), Name = "Adidas" },
                    new Brand { Id = Guid.Parse("DADC0B92-786A-4A12-93BA-7B886530339C"), Name = "Puma" },
                    new Brand { Id = Guid.Parse("99FBF381-B297-4B60-BE33-41CE92D61175"), Name = "Reebok" },
                    new Brand { Id = Guid.Parse("90050545-B51A-4E69-88B8-4B4FCFBAE671"), Name = "Gucci" },
                    new Brand { Id = Guid.Parse("E6AF6412-F80E-42ED-A11A-9C0C15D2549B"), Name = "Prada" },
                    new Brand { Id = Guid.Parse("B81D50FE-5A60-4836-B99D-861536F95C3B"), Name = "Louis Vuitton" },
                    new Brand { Id = Guid.Parse("2F47893F-C7A7-4E45-B873-B140CA66661A"), Name = "Zara" },
                    new Brand { Id = Guid.Parse("71A1034A-0B33-49F2-95ED-D55AA17F910B"), Name = "H&M" },
                    new Brand { Id = Guid.Parse("974F8947-A110-434A-96B6-171B0A5D9E28"), Name = "Levi's" }
                };

                _context.Brands.AddRange(brands);
                _context.SaveChanges();
            }
        }
    }
}
