using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Data.Helpers;

namespace Odin.Data.Persistence
{
    public class TransfereesRepository : ITransfereesRepository
    {
        private readonly ApplicationDbContext _context;

        public TransfereesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Transferee GetTransfereeByEmail(string email)
        {
            return _context.Transferees.SingleOrDefault(t => t.Email.Equals(email));
        }

        public void Add(Transferee transferee)
        {
            var userStore = new UserStore<Transferee>(_context);
            var userManager = new UserManager<Transferee>(userStore);

            userManager.Create(transferee);
            userManager.AddToRole(transferee.Id, UserRoles.Transferee);
        }
    }
}
