using Odin.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Odin.Data.Core.Models;


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
        public DateTime? FinalArrivalDate { get; set; }
        public DateTime? HomeFindingDate { get; set; }
        public DateTime? WorkStartDate { get; set; }

        public string SpouseName { get; set; }
        public string SpouseVisaType { get; set; }

        public string Rmc { get; set; }
        public string RmcContact { get; set; }
        public string RmcContactEmail { get; set; }


        public string TempHousingEndDateDisplay =>
            TempHousingEndDate.HasValue ? TempHousingEndDate.Value.ToString("d") : String.Empty;

        private IEnumerable<ChildViewModel> _children;
        public virtual IEnumerable<ChildViewModel> Children
        {
            get
            {
                return _children.Where(c => !c.Deleted).ToList();
            }
            set
            {
                _children = value;
            }
        }
        public string ChildrenEducationPreferences { get; set; }
        public string SchoolDistrict { get; set; }

        private IEnumerable<PetViewModel> _pets;
        public virtual IEnumerable<PetViewModel> Pets
        {
            get
            {
                return _pets.Where(p => !p.Deleted).ToList();
            }
            set
            {
                _pets = value;
            }
        }

        public string PetNotes { get; set; }

        public IEnumerable<ServiceViewModel> Services { get; set; }
        public IEnumerable<ServiceTypeViewModel> PossibleServices { get; set; }

        public TransfereeViewModel Transferee { get; set; }

        public int? RentId { get; set; }
        public Rent rent { get; set; }

        public string ProgramManagerId { get; set; }
        public virtual Manager ProgramManager { get; set; }
        public string SeCustNumb { get; set; }
        public string Client { get; set; }
        public string ClientFileNumber { get; set; }
    }
}