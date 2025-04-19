using DAL;

namespace Domain
{
    public class ProductDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal DollarPrice { get; set; }

        public string? UrlMainImage { get; set; } = string.Empty;

        public static ProductDTO FromProduct(Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                DollarPrice = product.DollarPrice,
                UrlMainImage = product.Images.FirstOrDefault(img => img.IsMain)?.ImageName
                      ?? product.Images.FirstOrDefault()?.ImageName
            };
        }

    }
}
