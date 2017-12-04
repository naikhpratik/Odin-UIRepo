using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Data.SqlClient;

namespace Odin.Data.Persistence
{
    public class UserNotificationRepository : IUserNotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public UserNotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserNotification> GetUserNotification(string UserID)
        {
            DateTime currentdate = Convert.ToDateTime(DateTime.Now.AddDays(-30));
            return _context.UserNotifications
                .Where(n => n.UserId == UserID && ((!n.IsRead) || (n.CreatedAt >= currentdate && n.IsRead)))
                .Include(n => n.Notification)
                .Include(n => n.Notification.Order)
                .ToList();

        }

        public IEnumerable<UserNotification> GetReadUserNotification(string UserID)
        {
            DateTime currentdate = Convert.ToDateTime(DateTime.Now.AddDays(-30));
            return _context.UserNotifications
                .Where(n => n.UserId == UserID && (n.CreatedAt >= currentdate && n.IsRead))
                .Include(n => n.Notification)
                .Include(n => n.Notification.Order)
                .ToList();

        }

        


        public UserNotification GetUserNotificationByNotificationId(string UserID, string NotificationId)
        {
            return _context.UserNotifications
                .Where(n => n.UserId == UserID && n.Id == NotificationId).SingleOrDefault();

        }


    }
}
