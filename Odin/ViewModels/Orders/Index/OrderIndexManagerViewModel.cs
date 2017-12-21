using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Odin.Data.Core.Models;
using Odin.ViewModels.Shared;

namespace Odin.ViewModels.Orders.Index
{
    public class OrderIndexManagerViewModel
    {

        public OrderIndexManagerViewModel()
        {

        }
        public OrderIndexManagerViewModel(IEnumerable<OrdersIndexViewModel> orderVms, IEnumerable<ManagerViewModel> managersVms)
        {
            OrdersIndexVm = orderVms;
            Managers = managersVms;
           // IEnumerable<ManagerViewModel> Managers = (ManagerViewModel)managersVms;
           
        }

        public IEnumerable<OrdersIndexViewModel> OrdersIndexVm { get; set; }
        public IEnumerable<ManagerViewModel> Managers { get; set; }

    }
}