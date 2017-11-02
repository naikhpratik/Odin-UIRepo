using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Persistence
{
    public class HomeFindingsRepository
    {
        private readonly IApplicationDbContext _context;

        public HomeFindingsRepository(IApplicationDbContext context)
        {
            _context = context;
        }
    }
}
