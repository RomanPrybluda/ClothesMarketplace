using DAL;

namespace Domain
{
    public class ProductByIdDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal DollarPrice { get; set; }

        public ICollection<Image> Images { get; set; } = new List<Image>();

        public Guid BrandId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid ForWhomId { get; set; }

        public Guid ColorId { get; set; }

        public Guid? ProductSizeId { get; set; }

        public Guid ProductConditionId { get; set; }

        public static ProductByIdDTO FromProduct(Product product)
        {
            return new ProductByIdDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                DollarPrice = product.DollarPrice,
                Images = product.Images,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                ForWhomId = product.ForWhomId,
                ColorId = product.ColorId,
                ProductSizeId = product.ProductSizeId,
                ProductConditionId = product.ProductConditionId,
            };
        }

    }
}
