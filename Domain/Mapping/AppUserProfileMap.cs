using AutoMapper;
using DAL;

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
