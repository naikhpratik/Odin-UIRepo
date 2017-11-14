using Odin.Data.Core.Models;
using Odin.Helpers;
using Odin.ViewModels.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PetViewModel = Odin.ViewModels.Shared.PetViewModel;


namespace Odin.ViewModels.Orders.Transferee
{
    public class OrdersTransfereeItineraryViewModel
    {
        public OrdersTransfereeItineraryViewModel()
        {
            
            Services = new List<ServiceViewModel>();            
            PossibleServices = new List<ServiceTypeViewModel>();
            HomeFinding = new HomeFindingViewModel();
        }

        public string Id { get; set; }      
        

        public DateTime? PreTripDate {get; set; }
        [DisplayName("Familiarization Trip:")]
        public string PreTripDateDisplay
        {
            get { return DateHelper.GetViewFormat(PreTripDate); }
        }

        [DisplayName("Trip Notes:")]
        public string PreTripNotes { get; set; }

        
        public DateTime? HomeFindingDate { get; set; }

        [DisplayName("Home Finding:")]
        public string HomeFindingDateDisplay
        {
            get { return DateHelper.GetViewFormat(HomeFindingDate); }
        }


        public DateTime? EstimatedArrivalDate {get; set; }

        [DisplayName("Final Arrival:")]
        public string EstimatedArrivalDateDisplay
        {
            get { return DateHelper.GetViewFormat(EstimatedArrivalDate); }
        }
        
        public DateTime? WorkStartDate { get; set; }

        [DisplayName("Work Start:")]
        public string WorkStartDateDisplay {
            get { return DateHelper.GetViewFormat(WorkStartDate); }
            
        }

        public DateTime? EstimatedDepartureDate {get; set; }

        [DisplayName("Estimated Departure:")]
        public string EstimatedDepartureDateDisplay
        {
            get { return DateHelper.GetViewFormat(EstimatedDepartureDate); }
        }
        
        public IEnumerable<ServiceViewModel> Services           { get; set; }

        public IEnumerable<ServiceTypeViewModel> PossibleServices { get; set; }
        

        public string RentId { get; set; }
        public HomeFindingViewModel HomeFinding { get; set; }


        [DisplayName("Comments:")]
        public string CommentsDisplay
        {
            get
            {
                return (HomeFinding != null && !String.IsNullOrEmpty(HomeFinding.Comments))
                    ? HomeFinding.Comments
                    : String.Empty;
            }
        }

    }
}