using AutoMapper;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping
{
    public class AppUserProfileMap : Profile
    {
        public AppUserProfileMap()
        {
            CreateMap<AppUserDTO, AppUser>().ReverseMap();
        }
    }
}
