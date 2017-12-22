﻿using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Odin.Data.Helpers;

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
            return _context.Orders

                .Where(o => o.ConsultantId == userId)
                .Include(o => o.Transferee)
                .Include(o => o.ProgramManager)
                .Include(o => o.Consultant)
                .Include(o => o.Services.Select(st => st.ServiceType))
                .ToList();

        }

        public IEnumerable<Order> GetOrdersFor(string userId, string userRole)
        {
            if (UserRoles.Transferee == userRole)
            {
                return _context.Orders
                    .Where(o => o.TransfereeId == userId)
                    .Include(o => o.Transferee)
                    .Include(o => o.ProgramManager)
                    .Include(o => o.Consultant)
                    .Include(o => o.Services.Select(st => st.ServiceType))
                    .ToList();
            }
            else if (UserRoles.Consultant == userRole)
            {
                return _context.Orders
                    .Where(o => o.ConsultantId == userId)
                    .Include(o => o.Transferee)
                    .Include(o => o.ProgramManager)
                    .Include(o => o.Consultant)
                    .Include(o => o.Services.Select(st => st.ServiceType))
                    .ToList();
            }
            else if (UserRoles.ProgramManager == userRole)
            {
                return _context.Orders
                    .Where(o => o.ProgramManagerId == userId)
                    .Include(o => o.Transferee)
                    .Include(o => o.ProgramManager)
                    .Include(o => o.Consultant)
                    .Include(o => o.Services.Select(st => st.ServiceType))
                    .ToList();
            }

            return null;
        }

        public Order GetOrderByTrackingId(string trackingId)
        {
            return _context.Orders
                .Where(o => o.TrackingId.Equals(trackingId))
                .Include(o => o.Services)
                .SingleOrDefault<Order>();
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

        public Order GetOrderById(string orderId)
        {

            return _context.Orders
                .Where(o => o.Id == orderId)
                .Include(o => o.Services)
                .Include(o => o.HomeFinding)
                .Include(o => o.Services.Select(s => s.ServiceType))
                .Include(o => o.HomeFinding)
                .Include(o => o.HomeFinding.NumberOfBathrooms)
                .Include(o => o.HomeFinding.HousingType)
                .Include(o => o.HomeFinding.AreaType)
                .Include(o => o.HomeFinding.TransportationType)
                .Include(o => o.DepositType)
                .Include(o => o.BrokerFeeType)
                .SingleOrDefault<Order>();
        }

        public Order GetOrderFor(string userId, string orderId)
        {
            
            return GetOrderFor(userId, orderId, UserRoles.Consultant);
        }

        public Order GetOrderFor(string userId, string orderId, string userRole)
        {
            if (userRole == UserRoles.Transferee)
            {
                return _context.Orders
                .Where(o => o.Id == orderId && o.TransfereeId == userId)
                .Include(o => o.Services)
                .Include(o => o.HomeFinding)
                .Include(o => o.Services.Select(s => s.ServiceType))
                .Include(o => o.HomeFinding)
                .Include(o => o.HomeFinding.NumberOfBathrooms)
                .Include(o => o.HomeFinding.HousingType)
                .Include(o => o.HomeFinding.AreaType)
                .Include(o => o.HomeFinding.TransportationType)
                .Include(o => o.HomeFinding.HomeFindingProperties.Select(hfp => hfp.Property.Photos))
                .Include(o => o.DepositType)
                .Include(o => o.BrokerFeeType)
                .Include(o => o.Notifications)
                .SingleOrDefault<Order>();
            }
            else if(userRole == UserRoles.ProgramManager) // for Program Manager
            {
                return _context.Orders
                .Where(o => o.Id == orderId && o.ProgramManagerId == userId)
                .Include(o => o.Services)
                .Include(o => o.HomeFinding)
                .Include(o => o.Services.Select(s => s.ServiceType))
                .Include(o => o.HomeFinding)
                .Include(o => o.HomeFinding.NumberOfBathrooms)
                .Include(o => o.HomeFinding.HousingType)
                .Include(o => o.HomeFinding.AreaType)
                .Include(o => o.HomeFinding.TransportationType)
                .Include(o => o.HomeFinding.HomeFindingProperties.Select(hfp => hfp.Property.Photos))
                .Include(o => o.DepositType)
                .Include(o => o.BrokerFeeType)
                .Include(o => o.Notifications)
                .SingleOrDefault<Order>();
            }
            else //for Consultant
            {
                return _context.Orders
                .Where(o => o.Id == orderId && o.ConsultantId == userId)
                .Include(o => o.Services)
                .Include(o => o.HomeFinding)
                .Include(o => o.Services.Select(s => s.ServiceType))
                .Include(o => o.HomeFinding)
                .Include(o => o.HomeFinding.NumberOfBathrooms)
                .Include(o => o.HomeFinding.HousingType)
                .Include(o => o.HomeFinding.AreaType)
                .Include(o => o.HomeFinding.TransportationType)
                .Include(o => o.HomeFinding.HomeFindingProperties.Select(hfp => hfp.Property.Photos))
                .Include(o => o.DepositType)
                .Include(o => o.BrokerFeeType)
                .Include(o => o.Notifications)
                .SingleOrDefault<Order>();
            }
        }

    

        //public IEnumerable<UserNotification> GetUserNotification(string userId, string orderid)
        //{
        //    return _context.Orders
        //        .Where(o => o.Id == orderid && o.ConsultantId == userId).ToList<UserNotification>();


        //}
    }
}
