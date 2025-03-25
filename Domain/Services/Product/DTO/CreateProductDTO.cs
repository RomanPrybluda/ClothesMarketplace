using DAL;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class CreateProductDTO
    {

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal DollarPrice { get; set; }

        [MinLength(3, ErrorMessage = "Min photo quantity is 3.")]
        [MaxLength(10, ErrorMessage = "Max photo quantity is 10.")]
        public List<ProductImageDTO> Images { get; set; } = new List<ProductImageDTO>();

        public Guid BrandId { get; set; }

        public Guid ColorId { get; set; }

        public Guid? ProductSizeId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid ForWhomId { get; set; }

        public Guid ProductConditionId { get; set; }

        public static Product ToProduct(CreateProductDTO request)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                DollarPrice = request.DollarPrice,
                Images = request.Images.Select(dto => new ProductImage
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = dto.ImageUrl,
                    ProductId = Guid.Empty
                }).ToList(),
                BrandId = request.BrandId,
                ColorId = request.ColorId,
                ProductSizeId = request.ProductSizeId,
                CategoryId = request.CategoryId,
                ForWhomId = request.ForWhomId,
                ProductConditionId = request.ProductConditionId
            };
        }

    }
}
