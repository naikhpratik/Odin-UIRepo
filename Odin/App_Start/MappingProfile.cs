using AutoMapper;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;

using Odin.ViewModels.Orders.Index;
using Odin.ViewModels.Orders.Transferee;
using Odin.ViewModels.Shared;

namespace Odin
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(m => m.Phone, opt => opt.MapFrom(src => src.PhoneNumber));

            /*ViewModel Mappings*/

            /*Shared*/
            CreateMap<Transferee, TransfereeViewModel>();
            CreateMap<Manager, ManagerViewModel>();
            CreateMap<Service, ServiceViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ServiceType.Name));
            CreateMap<Child, ChildViewModel>();
            CreateMap<Pet, PetViewModel>();
            CreateMap<ServiceType, ServiceTypeViewModel>();
            CreateMap<HomeFinding, HomeFindingViewModel>();

            /*Order - Index*/
            CreateMap<Order, OrdersIndexViewModel>();
            CreateMap<Order, OrdersTransfereeViewModel>();

            CreateMap<HomeFinding, HousingViewModel>();
            CreateMap<Order, HousingViewModel>();
                
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

            CreateMap<ChildDto, Child>();
            CreateMap<PetDto, Pet>();

            CreateMap<Order, TransfereeIndexDto>()
                .ForMember(m => m.FirstName, opt => opt.MapFrom(src => src.Transferee.FirstName))
                .ForMember(m => m.LastName, opt => opt.MapFrom(src => src.Transferee.LastName))
                .ForMember(m => m.Manager, opt => opt.MapFrom(src => src.ProgramManager.FullName))
                .ForMember(m => m.FinalArrival, opt => opt.MapFrom(src => src.EstimatedArrivalDate))
                .ForMember(m => m.Company, opt => opt.MapFrom(src => src.Client))
                .ForMember(m => m.ManagerPhone, opt => opt.MapFrom(src => src.ProgramManager.PhoneNumber))
                .ForMember(m => m.PreTrip, opt => opt.MapFrom(src => src.PreTripDate));

            CreateMap<Service, ServiceDto>();

            CreateMap<OrdersTransfereeIntakeDestinationDto,Order>();
            CreateMap<OrdersTransfereeIntakeOriginDto, Order>();
            CreateMap<OrdersTransfereeIntakeFamilyDto, Order>()
                .ForMember(opt => opt.Children, opt => opt.Ignore())
                .ForMember(opt => opt.Pets, opt => opt.Ignore());
            CreateMap<OrdersTransfereeIntakeServicesDto, Order>();
            CreateMap<OrdersTransfereeIntakeServiceDto, Service>();
            CreateMap<OrdersTransfereeIntakeRmcDto, Order>();
            CreateMap<OrdersTransfereeIntakeTempHousingDto, Order>();
            CreateMap<OrdersTransfereeIntakeRentDto, HomeFinding>();
            CreateMap<OrdersTransfereeIntakeLeaseDto, Order>();
            CreateMap<OrdersTransfereeDetailsServiceDto, Service>();
        }
    }
}