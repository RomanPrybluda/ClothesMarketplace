using AutoMapper;
using DAL;

namespace Domain.Mapping
{
    public class UserRegistrationProfileMap : Profile
    {
        public UserRegistrationProfileMap()
        {
            CreateMap<RegistrationDTO, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
