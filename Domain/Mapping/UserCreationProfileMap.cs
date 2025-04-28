using AutoMapper;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping
{
    public class UserCreationProfileMap : Profile
    {
        public UserCreationProfileMap() 
        {
            CreateMap<CreateAppUserDTO, AppUser>();
        }
    }
}
