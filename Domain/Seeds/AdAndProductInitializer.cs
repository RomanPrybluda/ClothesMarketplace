using DAL;

namespace Domain
{
    public class AdAndProductInitializer
    {

        private readonly ClothesMarketplaceDbContext _context;
        private static readonly Random _random = new Random();

        private readonly int needsProductsQuantity = 100;


        public AdAndProductInitializer(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public void InitializeAdsAndProducts()
        {
            var random = new Random();

            var brands = _context.Brands.ToList();
            var colors = _context.Colors.ToList();
            var categories = _context.Categories.ToList();
            var forWhomList = _context.ForWhoms.ToList();
            var productConditions = _context.ProductConditions.ToList();
            var deliveryMethods = _context.DeliveryMethods.ToList();

            if (!brands.Any() || !colors.Any() || !categories.Any() || !forWhomList.Any() || !productConditions.Any() || !deliveryMethods.Any())
            {
                throw new InvalidOperationException("Missing data in related tables!");
            }

            var ads = new List<Ad>();
            var products = new List<Product>();

            for (int i = 0; i < needsProductsQuantity; i++)
            {
                var brand = brands[_random.Next(brands.Count)];
                var color = colors[_random.Next(colors.Count)];
                var category = categories[_random.Next(categories.Count)];
                var forWhom = forWhomList[_random.Next(forWhomList.Count)];
                var condition = productConditions[_random.Next(productConditions.Count)];
                var deliveryMethod = deliveryMethods[_random.Next(deliveryMethods.Count)];

                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = $"Product {i + 1}",
                    Description = $"Description for Product {i + 1}",
                    DollarPrice = random.Next(10, 500),
                    LikesCount = random.Next(0, 1000),
                    BrandId = brand.Id,
                    ColorId = color.Id,
                    CategoryId = category.Id,
                    ForWhomId = forWhom.Id,
                    ProductConditionId = condition.Id,
                };

                var ad = new Ad
                {
                    Id = Guid.NewGuid(),
                    Title = $"Ad {i + 1}",
                    Description = $"Description for Ad {i + 1}",
                    IsActive = _random.Next(2) == 1,
                    ProductLocation = $"Location {_random.Next(1, 50)}",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Product = product,
                    DeliveryMethodId = deliveryMethod.Id
                };

                product.AdId = ad.Id;
                product.Ad = ad;

                products.Add(product);
                ads.Add(ad);
            }

            _context.Products.AddRange(products);
            _context.Ads.AddRange(ads);
            _context.SaveChanges();
        }
    }

}
