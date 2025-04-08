using AutoMapper;
using DAL;
using Domain.Services.Product.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping
{
    public class ProductCreationProfileMap : Profile
    {
        public ProductCreationProfileMap() 
        {
            CreateMap<CreateProductTest, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DollarPrice, opt => opt.MapFrom(src => src.DollarPrice))
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.ProductSizeId, opt => opt.MapFrom(src => src.ProductSizeId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.ForWhomId, opt => opt.MapFrom(src => src.ForWhomId))
                .ForMember(dest => dest.ProductConditionId, opt => opt.MapFrom(src => src.ProductConditionId));
        }
    }
}
