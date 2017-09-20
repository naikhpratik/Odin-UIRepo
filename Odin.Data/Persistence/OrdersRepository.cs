using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var orders = _context.Orders;
            return _context.Orders
                .Where(o => o.ConsultantId == userId)
                .Include(o => o.Transferee)
                .Include(o => o.ProgramManager)
                .Include(o => o.Consultant)
                .ToList();

        }

        public Order GetOrderByTrackingId(string trackingId)
        {
            return _context.Orders.SingleOrDefault(o => o.TrackingId.Equals(trackingId));
        }
    }
}
