using Odin.ViewModels.Shared;
using System.Collections.Generic;
using Odin.Data.Core.Models;
using System;

namespace Odin.ViewModels.Orders.Transferee
{
    public class OrdersTransfereeViewModel
    {
        public OrdersTransfereeViewModel()
        {
            Children = new List<ChildViewModel>();
            Services = new List<ServiceViewModel>();
            PossibleServices = new List<ServiceTypeViewModel>();
        }

        public string Id { get; set; }

        public string DestinationCity { get; set; }
        public string DestinationState { get; set; }
        public string DestinationCountry { get; set; }

        public string OriginCity { get; set; }
        public string OriginState { get; set; }
        public string OriginCountry { get; set; }

        public bool IsRush { get; set; }
        public bool IsVip { get; set; }

        public DateTime? PreTripDate { get; set; }
        public int TempHousingDays { get; set; }
        public DateTime? TempHousingEndDate { get; set; }

        public string SpouseName { get; set; }
        public string SpouseVisaType { get; set; }
        public virtual IEnumerable<ChildViewModel> Children { get; set; }
        public string ChildrenEducationPreferences { get; set; }
        public string SchoolDistrict { get; set; }

        public IEnumerable<ServiceViewModel> Services { get; set; }
        public IEnumerable<ServiceTypeViewModel> PossibleServices { get; set; }

        public TransfereeViewModel Transferee { get; set; }

        public int? RentId { get; set; }
        public Rent rent { get; set; }

        public string ProgramManagerId { get; set; }
        public virtual Manager ProgramManager { get; set; }
        public string SeCustNumb { get; set; }
        public string Rmc { get; set; }
        public string Client { get; set; }
        public string ClientFileNumber { get; set; }
    }
}