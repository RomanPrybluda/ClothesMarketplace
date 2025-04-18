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
    }
}
