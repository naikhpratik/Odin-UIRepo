using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Odin.ViewModels.Orders.Index;

namespace Odin.ViewModels.Orders.ProgramManager
{
    public class JoinedIndexViewModel
    {
        public IEnumerable<OrdersIndexViewModel> OrdersIndexViewModel { get; set; }
        public ProgramManagerViewModel ProgramManagerViewModel { get; set; }
    }
}