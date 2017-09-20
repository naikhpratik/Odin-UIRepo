using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;

namespace Odin.Data.Persistence
{
    public class ConsultantRepository : IConsultantRepository
    {
        private readonly ApplicationDbContext _context;

        public ConsultantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Consultant GetConsultantBySeContactUid(int seContactUid)
        {
            return _context.Consultants.FirstOrDefault(c => c.SeContactUid == seContactUid);
        }
    }
}
