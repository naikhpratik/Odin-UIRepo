using Odin.Data.Core.Models;
using System.Collections.Generic;

namespace Odin.Data.Core.Repositories
{
    public interface IOrdersRepository
    {
        IEnumerable<Order> GetOrdersFor(string userId,string userRole);
        Order GetOrderByTrackingId(string trackingId);
        void Add(Order order);
        Order GetOrderById(string orderId);
        Order GetOrderFor(string userId, string orderId,string userRole);
    }
}
