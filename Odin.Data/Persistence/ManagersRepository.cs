using Microsoft.AspNet.Identity;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Data.Helpers;
using System.Linq;

namespace Odin.Data.Persistence
{
    public class ManagersRepository : IManagersRepository
    {
        private readonly ApplicationDbContext _context;

        public ManagersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Manager GetManagerBySeContactUid(int seContactUid)
        {
            return _context.Managers.FirstOrDefault(m => m.SeContactUid == seContactUid);
        }

        public void Add(Manager manager, string userRole)
        {
            var userManager = UserHelper.GetUserManager<Manager>(_context);
            manager.UserName = manager.Email;
            var result = userManager.Create(manager, PasswordHelper.TemporaryPassword());
            userManager.AddToRole(manager.Id, userRole);
        }        
        
    }
}
