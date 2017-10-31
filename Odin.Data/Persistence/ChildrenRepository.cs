using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Linq;

namespace Odin.Data.Persistence
{
    public class ChildrenRepository : IChildrenRepository
    {
        private readonly IApplicationDbContext _context;

        public ChildrenRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public Child GetChildFor(string userId, string id)
        {
            return _context.Children.SingleOrDefault(c => c.Id == id && c.Order.ConsultantId == userId);
        }

    }
}
