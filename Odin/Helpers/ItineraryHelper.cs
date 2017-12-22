using AutoMapper;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.ViewModels.Orders.Transferee;
using Odin.ViewModels.Shared;
using System.Collections.Generic;
using System.Linq;

namespace Odin.Helpers
{
    public class ItineraryHelper
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItineraryHelper(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public OrdersTransfereeItineraryViewModel Build(string id)
        {
            var itinerary = GetItinerary(id);
            OrdersTransfereeItineraryViewModel vm = new OrdersTransfereeItineraryViewModel();
            vm.Itinerary = itinerary;
            return vm;
        }

        public IEnumerable<ItineraryEntryViewModel> GetItinerary(string id)
        {
            var itinService = _unitOfWork.Services.GetServicesByOrderId(id);
            var itinAppointments = _unitOfWork.Appointments.GetAppointmentsByOrderId(id);
            // NOTE: This works because homefinding id is the same as order id
            var itinViewings = _unitOfWork.HomeFindingProperties.GetUpcomingHomeFindingPropertiesByHomeFindingId(id);

            var itinerary1 = _mapper.Map<IEnumerable<Service>, IEnumerable<ItineraryEntryViewModel>>(itinService);
            var itinerary2 = _mapper.Map<IEnumerable<Appointment>, IEnumerable<ItineraryEntryViewModel>>(itinAppointments);
            var itinerary3 = _mapper.Map<IEnumerable<HomeFindingProperty>, IEnumerable<ItineraryEntryViewModel>>(itinViewings);

            var itinerary = itinerary1.Concat(itinerary2).Concat(itinerary3).OrderBy(s => s.ScheduledDate);
            return itinerary;
        }
    }
}