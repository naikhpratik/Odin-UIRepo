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
            CreateMap<ConsultantAssignment, ConsultantViewModel>()
                .ForMember(c => c.FirstName, opt => opt.MapFrom(src => src.Consultant.FirstName))
                .ForMember(c => c.LastName, opt => opt.MapFrom(src => src.Consultant.LastName));
            CreateMap<Transferee, TransfereeViewModel>();
        }
    }
}