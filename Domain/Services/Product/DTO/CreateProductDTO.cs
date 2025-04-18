using DAL;
using Microsoft.AspNetCore.Http;
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
        public List<IFormFile> Images { get; set; }

        public int MainImageIndex { get; set; } = 0;

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
