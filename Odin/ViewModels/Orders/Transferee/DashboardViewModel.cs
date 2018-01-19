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
            Services = new List<ServiceViewModel>();
            HomeFindingServices = new List<ServiceViewModel>();
        }

        public IEnumerable<ServiceViewModel> Services { get; set; }

        public IEnumerable<ServiceViewModel> HomeFindingServices { get; set; }

        private IEnumerable<ServiceViewModel> _allServices;
        public IEnumerable<ServiceViewModel> AllServices
        {
            get
            {
                if (_allServices == null)
                {
                    _allServices = Services.Concat(HomeFindingServices);
                }
                return _allServices;
            }
        }

        public IEnumerable<ServiceCategory> ServiceCategories
        {
            get
            {
                return AllServices.Where(s => s.Selected).Select(s => s.Category).Distinct().ToList();
            }
        }

        public IEnumerable<ServiceViewModel> GetServiceTypesByCategory(ServiceCategory cat)
        {
            return AllServices.Where(s => s.Category == cat && s.Selected).ToList();
        }

        public int CompletedServiceCount
        {
            get
            {
                return AllServices.Where(s => s.Selected && s.CompletedDate.HasValue).Count();
            }
        }

        public int TotalServiceCount
        {
            get
            {
                return AllServices.Where(s => s.Selected).Count();
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
