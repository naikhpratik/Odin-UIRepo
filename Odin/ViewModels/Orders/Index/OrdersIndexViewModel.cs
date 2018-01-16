﻿using Odin.Helpers;
using Odin.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Odin.Data.Core.Models;

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
        public int AuthorizedServicesDisplay => Services.Count();
        public int ScheduledServicesDisplay => Services.Where(s => s.ScheduledDate.HasValue && !s.CompletedDate.HasValue).Count();
        public int CompletedServicesDisplay => Services.Where(s => s.CompletedDate.HasValue).Count();
         

        public TransfereeViewModel Transferee { get; set; }

        public ManagerViewModel ProgramManager { get; set; }

        public IEnumerable<ServiceViewModel> Services { get; set; }

        public ICollection<Notification> Notifications { get; private set; }

        public int UserNotificationsCount { get; set; }

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


        public decimal CompletedWidth { get; set; }
        public decimal ScheduledWidth {  get; set; }
        public decimal AuthorizedWidth {  get; set; }


        // Call this function in Index inorder to set the CompletedWidth, ScheduledWidth, AuthorizedWidth
        public List<decimal> updateTask()
        {
            List<decimal> tasks = new List<decimal>();
            
            int at = 0;
            decimal s = 0;
            decimal c = 0;
            decimal cwid = 25;
            decimal swid = 50;
            decimal atwid = 100;
            for (int i = 0; i < Services.Count(); i++)
            {
                if (Services.ElementAt(i).Selected == true)
                {
                    at += 1;
                }
                if (Services.ElementAt(i).ScheduledDate.HasValue)
                {
                    s += 1;
                }
                if (Services.ElementAt(i).CompletedDate.HasValue)
                {
                    c += 1;
                }
            }
            tasks.Add(cwid = (at == 0 ? 0 : c / at * 100));
            tasks.Add(swid = (at == 0 ? 0 : s / at * 100));
            tasks.Add(atwid = swid == 0 ? 100 : 100);

            this.CompletedWidth = cwid;
            this.ScheduledWidth = swid;
            this.AuthorizedWidth = atwid;

            return tasks;
        }

    }
}