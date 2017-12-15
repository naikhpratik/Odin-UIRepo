using AutoMapper;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.ViewModels.BookMarklet;
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
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ServiceType.Name))
                .ForMember(dest => dest.ActionLabel, opt => opt.MapFrom(src => src.ServiceType.ActionLabel))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.ServiceType.Category));
            CreateMap<Child, ChildViewModel>();
            CreateMap<Pet, PetViewModel>();
            CreateMap<ServiceType, ServiceTypeViewModel>();
            CreateMap<HomeFinding, HomeFindingViewModel>();
            CreateMap<UserNotification, NotificationViewModel>();

            /*Order - Index*/
            CreateMap<Order, OrdersIndexViewModel>();
            CreateMap<Order, OrdersTransfereeViewModel>();

            /*Itinerary */
            CreateMap<Appointment, ItineraryEntryViewModel>()
                .ForMember(m => m.ItemType, opt => opt.MapFrom(src => src.GetType().ToString()))
                .ForMember(m => m.ScheduledDate, opt => opt.MapFrom(src => src.ScheduledDate))
                .ForMember(m => m.ActionLabel, opt => opt.MapFrom(src => src.Description));
            CreateMap<Service, ItineraryEntryViewModel>()
                .ForMember(m => m.ItemType, opt => opt.MapFrom(src => src.GetType().ToString()))
                .ForMember(m => m.ScheduledDate, opt => opt.MapFrom(src => src.ScheduledDate))
                .ForMember(m => m.ActionLabel, opt => opt.MapFrom(src => src.ServiceType.ActionLabel));
            CreateMap<HomeFindingProperty, ItineraryEntryViewModel>()
                .ForMember(m => m.ItemType, opt => opt.MapFrom(src => src.GetType().ToString()))
                .ForMember(m => m.ScheduledDate, opt => opt.MapFrom(src => src.ViewingDate))
                .ForMember(m => m.ActionLabel, opt => opt.MapFrom(src => ("Viewing " + src.Property.Street1 + " " + src.Property.Street2 + ", " + src.Property.City + " " + src.Property.State)));


            CreateMap<Order, HousingViewModel>();
            CreateMap<HomeFinding, HousingViewModel>();

            CreateMap<HomeFindingProperty, HousingPropertyViewModel>()
                .ReverseMap();

            CreateMap<Photo, PhotoViewModel>();

            /*BookMarklet*/
            CreateMap<Order, BookMarkletOrderViewModel>();

            /*DTO Mappings*/

            CreateMap<OrderDto, Order>()
                .ForMember(m => m.Transferee, opt => opt.Ignore())
                .ForMember(m => m.Consultant, opt => opt.Ignore())
                .ForMember(m => m.ProgramManager, opt => opt.Ignore())
                .ForMember(m => m.Appointments, opt => opt.Ignore())
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
            CreateMap<AppointmentDto, Appointment>();
            CreateMap<MessageDto, Message>();

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
            CreateMap<OrdersTransfereeIntakeHomeFindingDto, HomeFinding>();
            CreateMap<OrdersTransfereeIntakeLeaseDto, Order>();
            CreateMap<OrdersTransfereeDetailsServiceDto, Service>();
            CreateMap<OrdersTransfereeIntakeRelocationDto, Order>();
            CreateMap<BookMarkletDto, BookMarkletAddViewModel>();

            /*Web Jobs*/
            CreateMap<BookMarkletDto, PropBotJobQueueEntry>();
        }
    }
}
