using Odin.ViewModels.Shared;
using System.Collections.Generic;


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
        public virtual IEnumerable<ChildViewModel> Children { get; set; }
        public string ChildrenEducationPreferences { get; set; }

        public IEnumerable<ServiceViewModel> Services { get; set; }
        public IEnumerable<ServiceTypeViewModel> PossibleServices { get; set; }

        public TransfereeViewModel Transferee { get; set; }
    }
}