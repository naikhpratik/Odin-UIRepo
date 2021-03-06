﻿namespace Odin.ViewModels.Shared
{
    public class NotificationViewModel
    {

        public string NotificationMessage { get; set; }
        public string Id { get; set; }
        public string NotificationOrderId { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationOrderTransfereeFullName { get; set; }
        public bool IsRead { get; set; }
        public bool IsRemoved { get; set; }
        public string NotificationUserNotificationId { get; set; }
        public string NotificationNotificationType { get; set; }
        public string NotificationCreatedByFullName{ get; set;}
    }

}