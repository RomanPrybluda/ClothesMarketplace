namespace DAL
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal DollarPrice { get; set; }

        public int LikesCount { get; set; }

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        public Guid BrandId { get; set; }

        public Brand Brand { get; set; } = null!;

        public Guid ColorId { get; set; }

        public Color Color { get; set; } = null!;

        public Guid? ProductSizeId { get; set; }

        public ProductSize ProductSize { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; } = null!;

        public Guid ForWhomId { get; set; }

        public ForWhom ForWhom { get; set; } = null!;

        public Guid ProductConditionId { get; set; }

        public ProductCondition ProductCondition { get; set; } = null!;

        public Guid? ProductDetailsId { get; set; }

        public ProductDetails? ProductDetails { get; set; } = null;

        public string? SellerId { get; set; }

        public AppUser? Seller { get; set; }

        public string? BuyerId { get; set; }

        public AppUser? Buyer { get; set; }

        public ICollection<AppUser> FavoritedByUsers { get; set; } = new List<AppUser>();
    }
}
