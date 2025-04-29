using AutoMapper;
using DAL;

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
