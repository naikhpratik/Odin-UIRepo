using Odin.Data.Core.Models;
using Odin.Interfaces;
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
            // HomeFinding = new HomeFindingViewModel();
            Appointments = new List<Appointment>();
            Sorted = new List<object>();
        }

        public string Id { get; set; }      
              
        public IEnumerable<ServiceViewModel> Services { get; set; }
        //public HomeFindingViewModel HomeFinding { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
        public IEnumerable<object> Sorted { get; set; }
    }
}