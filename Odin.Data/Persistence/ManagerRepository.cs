using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;

namespace Odin.Data.Persistence
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly ApplicationDbContext _context;

        public ManagerRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public Manager GetManagerBySeContactUid(int seContactUid)
        {
            return _context.Managers.FirstOrDefault(m => m.SeContactUid == seContactUid);
        }
    }
}
