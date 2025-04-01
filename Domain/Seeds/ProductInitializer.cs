using DAL;

namespace Domain
{
    public class ProductInitializer
    {
        private readonly ClothesMarketplaceDbContext _context;
        private static readonly Random _random = new Random();

        private const int NeedsProductsQuantity = 100;
        private const double PurchaseProbability = 0.3;
        private const int MaxProductsPerUser = 10;

        private readonly string[] _productNames =
        {
            "Leather Jacket", "Denim Jacket", "Hoodie", "Sweatshirt", "Turtleneck",
            "T-Shirt", "Polo Shirt", "Dress Shirt", "Blazer", "Trench Coat",
            "Jeans", "Chinos", "Cargo Pants", "Shorts", "Joggers",
            "Sneakers", "Loafers", "Boots", "Sandals", "Formal Shoes",
            "Backpack", "Leather Handbag", "Tote Bag", "Crossbody Bag", "Messenger Bag",
            "Sunglasses", "Wristwatch", "Scarf", "Belt", "Gloves"
        };

        public ProductInitializer(ClothesMarketplaceDbContext context)
        {
            _context = context;
        }

        public void InitializeProducts()
        {
            if (_context.Products.Count() >= NeedsProductsQuantity)
                return;

            var brands = _context.Brands.ToList();
            var colors = _context.Colors.ToList();
            var categories = _context.Categories.ToList();
            var forWhomList = _context.ForWhoms.ToList();
            var productConditions = _context.ProductConditions.ToList();
            var deliveryMethods = _context.DeliveryMethods.ToList();
            var productSizes = _context.ProductSizes.ToList();
            var users = _context.Users.ToList();

            var products = new List<Product>();

            foreach (var user in users)
            {
                int productsToCreate = _random.Next(1, MaxProductsPerUser + 1);

                for (int i = 0; i < productsToCreate && products.Count < NeedsProductsQuantity; i++)
                {
                    var productId = Guid.NewGuid();
                    var productDetailsId = Guid.NewGuid();

                    var product = new Product
                    {
                        Id = productId,
                        Name = _productNames[_random.Next(_productNames.Length)],
                        Description = "High-quality product for all occasions.",
                        DollarPrice = _random.Next(10, 500),
                        LikesCount = _random.Next(0, 1000),
                        BrandId = brands[_random.Next(brands.Count)].Id,
                        ColorId = colors[_random.Next(colors.Count)].Id,
                        CategoryId = categories[_random.Next(categories.Count)].Id,
                        ForWhomId = forWhomList[_random.Next(forWhomList.Count)].Id,
                        ProductConditionId = productConditions[_random.Next(productConditions.Count)].Id,
                        ProductSizeId = productSizes.Any() ? productSizes[_random.Next(productSizes.Count)].Id : (Guid?)null,
                        ProductDetailsId = productDetailsId,
                        SellerId = user.Id
                    };

                    var productDetails = new ProductDetails
                    {
                        Id = productDetailsId,
                        Title = "Product for sale",
                        Description = "Brand new product, available now!",
                        IsActive = true,
                        ProductLocation = $"Location {_random.Next(1, 50)}",
                        DeliveryMethodId = deliveryMethods[_random.Next(deliveryMethods.Count)].Id
                    };

                    _context.ProductDetails.Add(productDetails);

                    if (_random.NextDouble() < PurchaseProbability)
                    {
                        var buyers = users.Where(u => u.Id != user.Id).ToList();
                        if (buyers.Any())
                        {
                            var buyer = buyers[_random.Next(buyers.Count)];
                            product.BuyerId = buyer.Id;
                        }
                    }

                    products.Add(product);
                }
            }

            _context.Products.AddRange(products);
            _context.SaveChanges();
        }
    }
}
