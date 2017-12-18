using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Data.Core.Models;
using Odin.ViewModels.Orders.Index;
using Odin;

namespace Odin.Tests.ViewModels.Orders.Index
{
    [TestClass]
    public class OrdersIndexViewModelTests
    {
        private Order order;
        private IMapper mapper;

        [TestInitialize]
        public void TestInit()
        {
            var orderId = "1";
            order = new Order() { Id = orderId };
            order.HomeFinding = new HomeFinding();

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            mapper = config.CreateMapper();
        }

        [TestMethod]
        public void OrderIndexViewModelUpdatesTasks()
        {
            order.Services.Add(new Service() { Id = "1", ScheduledDate = DateTime.Now, CompletedDate = null, Selected = true });
            order.Services.Add(new Service() { Id = "2", ScheduledDate = DateTime.Now, CompletedDate = DateTime.Now, Selected = false });
            order.Services.Add(new Service() { Id = "3", ScheduledDate = DateTime.Now, CompletedDate = DateTime.Now, Selected = false});
            OrdersIndexViewModel viewModel = mapper.Map<Order, OrdersIndexViewModel>(order);

            List<decimal> tasks = viewModel.updateTask();
            List<decimal> Test_task = new List<decimal>();
            Test_task.Add(200);
            Test_task.Add(300);
            Test_task.Add(100);

            tasks.Equals(Test_task);
            
        }
    }
}
