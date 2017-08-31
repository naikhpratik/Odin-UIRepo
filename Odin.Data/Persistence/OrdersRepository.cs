using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;

namespace Odin.Data.Persistence
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IApplicationDbContext _context;

        public OrdersRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetOrdersFor(string userId)
        {
            return _context.ConsultantAssignments
                .Where(ca => ca.ConsultantId == userId)
                .Select(ca => ca.Order)
                .ToList();
        }
    }
}
