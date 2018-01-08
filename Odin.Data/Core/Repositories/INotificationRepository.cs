using System.Collections.Generic;
using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetOrderNotifications(string orderid);
    }
}