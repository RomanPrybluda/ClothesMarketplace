using AutoMapper;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapping
{
    public class UpdateUserProfileMap : Profile
    {
        public UpdateUserProfileMap() 
        {
            CreateMap<UpdateAppUserDTO, AppUser>();
        }
    }
}
