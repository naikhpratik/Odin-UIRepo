using Odin.Data.Core.Models;
using Odin.Helpers;
using Odin.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Odin.ViewModels.Orders.Transferee
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            Services = new List<Service>();
        }

        public IEnumerable<Service> Services { get; set; }

        public IEnumerable<ServiceCategory> ServiceCategories
        {
            get { return Services.Where(s => s.Selected).Select(s => s.ServiceType.Category).Distinct(); }
        }

        public IEnumerable<Service> GetServiceTypesByCategory(ServiceCategory cat)
        {
            return Services.Where(s => s.ServiceType.Category == cat && s.Selected).ToList();
        }

        public int CompletedServiceCount
        {
            get
            {
                return Services.Where(s => s.Selected && s.CompletedDate.HasValue).Count();
            }
        }

        public int TotalServiceCount
        {
            get
            {
                return Services.Where(s => s.Selected).Count();
            }
        }

        public int PercentComplete
        {
            get { return Convert.ToInt32(Convert.ToDecimal(CompletedServiceCount) / Convert.ToDecimal(TotalServiceCount) * 100); }
        }

        public IEnumerable<ItineraryEntryViewModel> Itinerary { get; set; }

        public string GetServiceCategoryString(ServiceCategory cat)
        {
            return ServiceHelper.ToServiceCategoryString(cat);
        }
    }
}
