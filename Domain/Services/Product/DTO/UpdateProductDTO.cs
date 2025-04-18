using DAL;

namespace Domain
{
    public class UpdateProductDTO
    {

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal DollarPrice { get; set; }

        public List<ProductImageDTO> Images { get; set; }

        public Guid BrandId { get; set; }

        public Guid ColorId { get; set; }

        public Guid? ProductSizeId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid ForWhomId { get; set; }

        public Guid ProductConditionId { get; set; }

        public void UpdateProduct(Product product)
        {
            product.Name = Name;
            product.Description = Description;
            product.DollarPrice = DollarPrice;
            product.Images = Images.Select(img => new Image
            {
                ImageUrl = img.ImageUrl,
                IsMain = img.IsMain,
                ProductId = product.Id
            }).ToList();
            product.BrandId = BrandId;
            product.ColorId = ColorId;
            product.ProductSizeId = ProductSizeId;
            product.CategoryId = CategoryId;
            product.ForWhomId = ForWhomId;
            product.ProductConditionId = ProductConditionId;
        }

    }
}
