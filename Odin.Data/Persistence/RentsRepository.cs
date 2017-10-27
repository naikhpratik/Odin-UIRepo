using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Persistence
{
    public class RentsRepository
    {
        private readonly IApplicationDbContext _context;

        public RentsRepository(IApplicationDbContext context)
        {
            _context = context;
        }
    }
}
