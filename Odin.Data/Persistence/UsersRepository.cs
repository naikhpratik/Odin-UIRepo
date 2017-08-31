﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;

namespace Odin.Data.Persistence
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