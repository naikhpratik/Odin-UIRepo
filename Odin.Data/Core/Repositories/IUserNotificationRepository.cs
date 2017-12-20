using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface IUserNotificationRepository
    {
        IEnumerable<UserNotification> GetUserNotification(string UserID);
        UserNotification GetUserNotificationByNotificationId(string UserID, string NotificationId);
        IEnumerable<UserNotification> GetUserNotificationHistory(string UserID, string orderid);
        

    }
}
