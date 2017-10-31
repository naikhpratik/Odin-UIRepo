using Odin.Data.Builders;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using System.Data.Entity;
using System.Linq;

namespace Odin.IntegrationTests.Helpers
{
    public static class OrderHelper
    {
        public static Order CreateOrder(Consultant consultant = null, Manager manager = null,
            Transferee transferee = null, string TrackingId = null)
        {
            var order = OrderBuilder.New().First();
            order.TrackingId = TrackingId;
            order.Consultant = consultant;
            order.ProgramManager = manager;
            order.Transferee = transferee;
            return order;
        }

        public static OrderDto CreateOrderDto(ConsultantDto consultantDto = null,
            ProgramManagerDto programManagerDto = null, TransfereeDto transfereeDto = null, string TrackingId = null)
        {
            var orderDto = OrderDtoBuilder.New();
            orderDto.TrackingId = TrackingId;
            orderDto.Consultant = consultantDto;
            orderDto.ProgramManager = programManagerDto;
            orderDto.Transferee = transfereeDto;
            return orderDto;
        }

        public static void ClearIntegrationOrders(ApplicationDbContext context)
        {
            
            var transferees = context.Transferees.Where(t => t.Email.Contains("integration")).Include(t => t.Orders).ToList();

            if (transferees.Any())
            {
                foreach (var transferee in transferees)
                {
                    context.Orders.RemoveRange(transferee.Orders);
                }
                context.Transferees.RemoveRange(transferees);
            }

            var managers = context.Managers.Where(m => m.Email.Contains("integration")).Include(m => m.Orders).ToList();
            if (managers.Any())
            {
                foreach (var manager in managers)
                {
                    context.Orders.RemoveRange(manager.Orders);
                }
                context.Managers.RemoveRange(managers);
            }

            var consultants = context.Consultants.Where(c => c.Email.Contains("integration")).Include(c => c.Orders)
                .ToList();

            if (consultants.Any())
            {
                foreach (var consultant in consultants)
                {
                    context.Orders.RemoveRange(consultant.Orders);
                }
                context.Consultants.RemoveRange(consultants);
            }

            var orders = context.Orders
                .Where(o => o.TrackingId.Contains("integration") || o.DestinationCity.Contains("integration"))
                .Include(o => o.Transferee)
                .Include(s => s.Services)
                .Include(o => o.Rent)
                .Include(o => o.Pets)
                .Include(o => o.Children)
                .Include(o => o.Lease)
                .ToList();

            foreach (var order in orders)
            {
                context.Orders.Remove(order);
                if (order.Transferee != null)
                {
                    context.Transferees.Remove(order.Transferee);
                }
                if (order.Services != null)
                {
                    foreach (var service in order.Services)
                    {
                        context.Services.Remove(service);
                    }
                }
                if (order.Pets != null)
                {
                    foreach (var pet in order.Pets)
                    {
                        context.Pets.Remove(pet);
                    }
                }
                if (order.Children != null)
                {
                    foreach (var orderChild in order.Children)
                    {
                        context.Children.Remove(orderChild);
                    }
                }
                if (order.Lease != null)
                    context.Leases.Remove(order.Lease);
                if (order.Rent != null)
                    context.Rents.Remove(order.Rent);
            }             

            context.SaveChanges();
        }
    }
}
