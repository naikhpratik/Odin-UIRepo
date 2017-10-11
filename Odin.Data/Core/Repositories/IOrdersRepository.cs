using Odin.Data.Core.Models;
using System.Collections.Generic;

namespace Odin.Data.Core.Repositories
{
    public interface IOrdersRepository
    {
        IEnumerable<Order> GetOrdersFor(string userId);
        Order GetOrderByTrackingId(string trackingId);
        void Add(Order order);
        Order GetOrderById(int orderId);
    }
}
