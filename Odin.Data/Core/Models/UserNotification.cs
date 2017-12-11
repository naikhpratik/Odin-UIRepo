using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Models
{
    public class UserNotification : MobileTable
    {
        public string UserId { get; private set; }

        public string NotificationId { get; private set; }

        public ApplicationUser User { get; private set; }

        public Notification Notification { get; private set; }

        public bool IsRead { get; private set; }

        public bool IsRemoved { get; private set; }

        public UserNotification()
        {
        }

        public UserNotification(ApplicationUser user, Notification notification)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (notification == null)
                throw new ArgumentNullException("notification");

            User = user;
            UserId = user.Id;
            Notification = notification;
            NotificationId = notification.Id;
            
        }

        public void Read()
        {
            IsRead = true;
        }

        public void Remove()
        {
            IsRemoved = true;
        }
    }
}
