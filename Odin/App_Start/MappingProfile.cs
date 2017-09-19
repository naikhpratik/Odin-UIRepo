using AutoMapper;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.ViewModels;

namespace Odin
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(m => m.Phone, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<Order, OrderIndexViewModel>();
            CreateMap<ApplicationUser, TransfereeViewModel>();

            CreateMap<OrderDto, Order>();
            CreateMap<TransfereeDto, ApplicationUser>();
            CreateMap<ProgramManagerDto, ApplicationUser>();
        }
    }
}