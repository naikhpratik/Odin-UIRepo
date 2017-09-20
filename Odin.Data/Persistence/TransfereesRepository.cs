using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;

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
    }
}
