using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Odin.Data.Persistence
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Notification> GetOrderNotifications(string orderid)
        {
            return _context.Notifications
                .Where(n => n.OrderId == orderid)
                .Include(n => n.Order)
                .Include(n => n.CreatedBy)
                .ToList();
        }
    }
}
