using AutoMapper;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;

namespace Odin
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(m => m.Phone, opt => opt.MapFrom(src => src.PhoneNumber));
        }
    }
}