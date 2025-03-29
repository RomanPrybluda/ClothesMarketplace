using DAL;

namespace Domain
{
    public class AdAndProductInitializer
    {
        private readonly ClothesMarketplaceDbContext _context;
        private static readonly Random _random = new Random();

        private const int NeedsProductsQuantity = 100;

        private readonly string[] _productNames =
        {
            "Leather Jacket", "Denim Jacket", "Hoodie", "Sweatshirt", "Turtleneck",
            "T-Shirt", "Polo Shirt", "Dress Shirt", "Blazer", "Trench Coat",
            "Jeans", "Chinos", "Cargo Pants", "Shorts", "Joggers",
            "Sneakers", "Loafers", "Boots", "Sandals", "Formal Shoes",
            "Backpack", "Leather Handbag", "Tote Bag", "Crossbody Bag", "Messenger Bag",
            "Sunglasses", "Wristwatch", "Scarf", "Belt", "Gloves"
        };

        public AdAndProductInitializer(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public void InitializeAdsAndProducts()
        {
            int existingProductCount = _context.Products.Count();

            if (existingProductCount >= NeedsProductsQuantity)
                return;

            int productsToAdd = NeedsProductsQuantity - existingProductCount;

            var brands = _context.Brands.ToList();
            var colors = _context.Colors.ToList();
            var categories = _context.Categories.ToList();
            var forWhomList = _context.ForWhoms.ToList();
            var productConditions = _context.ProductConditions.ToList();
            var deliveryMethods = _context.DeliveryMethods.ToList();
            var productSizes = _context.ProductSizes.ToList();

            if (!brands.Any() || !colors.Any() || !categories.Any() || !forWhomList.Any() || !productConditions.Any() || !deliveryMethods.Any() || !productSizes.Any())
            {
                throw new InvalidOperationException("Missing data in related tables!");
            }

            var products = new List<Product>();
            var ads = new List<Ad>();

            for (int i = 0; i < productsToAdd; i++)
            {
                var brand = brands[_random.Next(brands.Count)];
                var color = colors[_random.Next(colors.Count)];
                var category = categories[_random.Next(categories.Count)];
                var forWhom = forWhomList[_random.Next(forWhomList.Count)];
                var condition = productConditions[_random.Next(productConditions.Count)];
                var deliveryMethod = deliveryMethods[_random.Next(deliveryMethods.Count)];
                var size = productSizes[_random.Next(productSizes.Count)];

                string productName = _productNames[_random.Next(_productNames.Length)];

                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = productName,
                    Description = $"High-quality {productName} for all occasions.",
                    DollarPrice = _random.Next(10, 500),
                    LikesCount = _random.Next(0, 1000),
                    BrandId = brand.Id,
                    ColorId = color.Id,
                    CategoryId = category.Id,
                    ForWhomId = forWhom.Id,
                    ProductConditionId = condition.Id,
                    ProductSizeId = size.Id,
                };

                var ad = new Ad
                {
                    Id = Guid.NewGuid(),
                    Title = $"{productName} for sale",
                    Description = $"Brand new {productName}, available now!",
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
