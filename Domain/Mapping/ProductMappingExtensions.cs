using DAL;
using Domain.Services.Product.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping
{
    public static class ProductMappingExtensions
    {
       public static Product ToProduct(this CreateProductTest request)
       {
            return new Product
            {
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

       public static ProductDetailsDto ToProductDetailsDto(this Product product)
        {
            return new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                DollarPrice = product.DollarPrice,
                LikesCount = product.LikesCount,
                Images = product.Images.Select(i => new ImageDto() {Url=i.ImageUrl, IsMain = i.IsMain }).ToList(),
                Brand = product.Brand.Name,
                Color = product.Color.Name,
                ProductSize = product.ProductSize.Value,
                Category = product.Category.Name,
                ForWhom = product.ForWhom.Name,
                ProductCondition = product.ProductCondition.Name
            };
        }
    }
}
