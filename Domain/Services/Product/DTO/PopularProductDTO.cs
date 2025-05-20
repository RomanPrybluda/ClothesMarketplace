using DAL;

namespace Domain
{
    public class PopularProductDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal DollarPrice { get; set; }

        public int LikesCount { get; set; }

        public string? UrlMainImage { get; set; } = string.Empty;

        public static PopularProductDTO FromProduct(Product product)
        {
            return new PopularProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                DollarPrice = product.DollarPrice,
                LikesCount = product.LikesCount,
                UrlMainImage = product.Images.FirstOrDefault(img => img.IsMain)?.ImageName
                    ?? product.Images.FirstOrDefault()?.ImageName
            };
        }
    }
}
