using AutoMapper;
using DAL;

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
