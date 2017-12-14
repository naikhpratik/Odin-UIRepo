using Odin.Helpers;
using System;
using Odin.Data.Core.Models;
using AutoMapper;
using System.Collections;
using System.Collections.Generic;
using Odin.ViewModels.Shared;

namespace Odin.ViewModels.Orders.Transferee
{
    public class HistoryViewModel
    {
        public string NotificationMessage { get; set; }
        public string Id { get; set; }
        public string NotificationOrderId { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationOrderTransfereeFullName { get; set; }
        public bool IsRead { get; set; }
        public bool IsRemoved { get; set; }
        public string NotificationUserNotificationId { get; set; }


        public NotificationType NotificationNotificationType { get; set; }

        public DateTime? NotificationCreatedAt { get; set; }

        public string MyNotificationCreatedAt
        {
            get
            {
                return DateHelper.GetViewHistoryFormat(this.NotificationCreatedAt);
            }

        }



    }

}