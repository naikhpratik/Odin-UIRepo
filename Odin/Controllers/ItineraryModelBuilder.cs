using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Odin.Data.Core;
using Odin.Interfaces;
using Odin.ViewModels.Orders.Transferee;
using Odin.ViewModels.Shared;
using Odin.Data.Core.Models;

namespace Odin.Controllers
{
    public class ItineraryModelBuilder: IItineraryModelBuilder<OrdersTransfereeItineraryViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItineraryModelBuilder(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public OrdersTransfereeItineraryViewModel Build(string id)
        {            
            var itinService = _unitOfWork.Services.GetServicesByOrderId(id);
            var itinAppointments = _unitOfWork.Appointments.GetAppointmentsByOrderId(id);
            //var itinViewings = _unitOfWork.HousingTypes.GetViewingsByOrderId(id);
            var itinerary1 = _mapper.Map<IEnumerable<Service>, IEnumerable<ItineraryEntryViewModel>>(itinService);
            var itinerary2 = _mapper.Map<IEnumerable<Appointment>, IEnumerable<ItineraryEntryViewModel>>(itinAppointments);
            //var itinerary3 = _mapper.Map<IEnumerable<HousingPropertyViewModel>, IEnumerable<ItineraryEntryViewModel>>(itinViewings);
            //var itinerary = itinerary1.Concat(itinerary2).Concat(itinerary3).OrderBy(s => s.ScheduledDate);
            var itinerary = itinerary1.Concat(itinerary2).OrderBy(s => s.ScheduledDate);
            OrdersTransfereeItineraryViewModel vm = new OrdersTransfereeItineraryViewModel();
            vm.Itinerary = itinerary;
            return vm;            
        }
    }
}