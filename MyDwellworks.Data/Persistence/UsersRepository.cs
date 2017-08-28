using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDwellworks.Data.Core.Models;
using MyDwellworks.Data.Core.Repositories;

namespace MyDwellworks.Data.Persistence
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationDbContext _context;

        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetUsersWithRole(string roleName)
        {
            var role = _context.Roles.Single(r => r.Name == roleName);
            return _context.Users.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.RoleId == role.Id));
        }
    }
}
