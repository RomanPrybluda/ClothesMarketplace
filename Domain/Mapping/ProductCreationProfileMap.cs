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
            CreateMap<CreateProductTest, Product>().ReverseMap();
        }
    }
}
