using Odin.Data.Core.Models;
using Odin.Helpers;
using Odin.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Odin.ViewModels.Orders.Index
{
    public class OrdersIndexViewModel
    {
        public string Id { get; set; }
        public string SeCustNumb { get; set; }
        public string Rmc { get; set; }
        public string Client { get; set; }
        public DateTime? PreTripDate { get; set; }
        public DateTime? EstimatedArrivalDate { get; set; }
        public DateTime? LastContactedDate { private get; set; }
        public bool IsRush { private get; set; }

        public string PreTripDateDisplay => DateHelper.GetViewFormat(PreTripDate);
        public string EstimatedArrivalDateDisplay => DateHelper.GetViewFormat(EstimatedArrivalDate);
        public string LastContactedDateDisplay => DateHelper.GetViewFormat(LastContactedDate);
        public string IsRushDisplay => IsRush ? "Rush" : String.Empty;
        public int AuthorizedServicesDisplay => AuthServices.Count();
        public int ScheduledServicesDisplay => AuthServices.Where(s => s.ScheduledDate.HasValue && !s.CompletedDate.HasValue).Count();
        public int CompletedServicesDisplay => AuthServices.Where(s => s.CompletedDate.HasValue).Count();
        public int CompletedWidth => Convert.ToInt32(AuthorizedServicesDisplay == 0 ? 0 : CompletedServicesDisplay * 100 / AuthorizedServicesDisplay);
        public int ScheduledWidth => Convert.ToInt32(AuthorizedServicesDisplay == 0 ? 0 : ScheduledServicesDisplay * 100 / AuthorizedServicesDisplay);
        public decimal AuthorizedWidth = 100;

        public TransfereeViewModel Transferee { get; set; }

        public ManagerViewModel ProgramManager { get; set; }

        public IEnumerable<ServiceViewModel> Services { get; set; }

        public ICollection<Notification> Notifications { get; private set; }

        public int UserNotificationsCount { get; set; }

        public IEnumerable<ServiceViewModel> HomeFindingServices { get; set; }

        private IEnumerable<ServiceViewModel> _authServices;
        public IEnumerable<ServiceViewModel> AuthServices
        {
            get
            {
                if (_authServices == null)
                {
                    _authServices = Services.Concat(HomeFindingServices).Where(s => s.Selected);
                }
                return _authServices;
            }
        }

        //public IEnumerable<UserNotification> GetOrderUserNotification()
        //{
        //    DateTime currentdate = Convert.ToDateTime(DateTime.Now.AddDays(-1));
        //    return UserNotifications.Where(u => u.CreatedAt == currentdate);
        //}

        //public IEnumerable<Notification> GetOrderNotifications()
        //{
        //    DateTime currentdate = Convert.ToDateTime(DateTime.Now.AddDays(-1));
        //    return Notifications.Where(n => n.CreatedAt == currentdate);
        //}

    }
}