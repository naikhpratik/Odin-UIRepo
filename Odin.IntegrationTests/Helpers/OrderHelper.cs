using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Builders;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Helpers;
using Odin.Data.Persistence;

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
            
            var orders = context.Orders.Where(o => o.TrackingId.Contains("integration")).ToList();
            foreach (var order in orders)
            {
                context.Transferees.Remove(order.Transferee);
                context.Orders.Remove(order);
            }

            context.SaveChanges();
        }
    }
}
