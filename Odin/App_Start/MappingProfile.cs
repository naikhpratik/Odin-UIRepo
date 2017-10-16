using AutoMapper;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.ViewModels.Order.Index;

namespace Odin
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(m => m.Phone, opt => opt.MapFrom(src => src.PhoneNumber));

            /*ViewModel Mappings*/

            /*Order - Index*/
            CreateMap<Order, OrderIndexViewModel>();
            CreateMap<Transferee, OrderIndexTransfereeViewModel>();            
            CreateMap<Manager, OrderIndexManagerViewModel>();
            CreateMap<Service, OrderIndexServiceViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ServiceType.Name));


            /*DTO Mappings*/

            CreateMap<OrderDto, Order>()
                .ForMember(m => m.Transferee, opt => opt.Ignore())
                .ForMember(m => m.Consultant, opt => opt.Ignore())
                .ForMember(m => m.ProgramManager, opt => opt.Ignore())
                .ForMember(m => m.Services, opt => opt.Ignore());
            CreateMap<TransfereeDto, Transferee>();
            CreateMap<ProgramManagerDto, Manager>();
            CreateMap<ConsultantDto, Consultant>();
            CreateMap<Transferee, TransfereeDto>();
            CreateMap<Manager, ProgramManagerDto>();
            CreateMap<Consultant, ConsultantDto>();
            CreateMap<ManagerDto, Manager>();
            CreateMap<ConsultantImportDto, Consultant>();
            CreateMap<ServiceDto, Service>();

            CreateMap<Order, TransfereeIndexDto>()
                .ForMember(m => m.FirstName, opt => opt.MapFrom(src => src.Transferee.FirstName))
                .ForMember(m => m.LastName, opt => opt.MapFrom(src => src.Transferee.LastName))
                .ForMember(m => m.Manager, opt => opt.MapFrom(src => src.ProgramManager.FullName))
                .ForMember(m => m.FinalArrival, opt => opt.MapFrom(src => src.EstimatedArrivalDate))
                .ForMember(m => m.Company, opt => opt.MapFrom(src => src.Client))
                .ForMember(m => m.ManagerPhone, opt => opt.MapFrom(src => src.ProgramManager.PhoneNumber))
                .ForMember(m => m.PreTrip, opt => opt.MapFrom(src => src.PreTripDate));

            CreateMap<Service, ServiceDto>();
        }
    }
}