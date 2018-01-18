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
    }
}
