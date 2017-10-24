using Odin.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;


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

        public string SpouseName { get; set; }
        public string SpouseVisaType { get; set; }

        public string Rmc { get; set; }
        public string RmcContact { get; set; }
        public string RmcContactEmail { get; set; }

        public int TempHousingDays { get; set; }
        public DateTime? TempHousingEndDate { get; set; }

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
    }
}