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
    public class ConsultantsRepository : IConsultantsRepository
    {
        private readonly ApplicationDbContext _context;

        public ConsultantsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Consultant GetConsultantBySeContactUid(int seContactUid)
        {
            return _context.Consultants.FirstOrDefault(c => c.SeContactUid == seContactUid);
        }

        public void Add(Consultant consultant)
        {
            var userStore = new UserStore<Consultant>(_context);
            var userManager = new UserManager<Consultant>(userStore);
            consultant.UserName = consultant.Email;
            var result = userManager.Create(consultant, TokenHelper.NewToken());
            userManager.AddToRole(consultant.Id, UserRoles.Consultant);
        }
    }
}
