using Odin.Helpers;
using System;

namespace Odin.ViewModels.Orders.Transferee
{
    public class HistoryViewModel
    {
        public string Message { get; set; }
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string Title { get; set; }
        public string OrderTransfereeFullName { get; set; }

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