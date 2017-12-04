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
             Itinerary = new List<ItineraryEntryViewModel>();
        }

        public string Id { get; set; }
        public bool mustPrint { get; set; }
        public IEnumerable<ItineraryEntryViewModel> Itinerary { get; set; }
    }
}