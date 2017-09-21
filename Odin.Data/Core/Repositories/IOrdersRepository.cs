using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface IOrdersRepository
    {
        IEnumerable<Order> GetOrdersFor(string userId);
        Order GetOrderByTrackingId(string trackingId);
        void Add(Order order);
    }
}
