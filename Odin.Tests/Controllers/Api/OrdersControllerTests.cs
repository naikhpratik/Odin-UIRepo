using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AutoMapper;
using FluentAssertions;
using System.Net;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Interfaces;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Odin.Tests.Controllers.Api
{
    [TestClass]
    public class OrdersControllerTests
    {
        private Odin.Controllers.Api.OrdersController _controller;
        private Mock<IOrdersRepository> _mockRepository;
        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IOrdersRepository>();
            _mockMapper = new Mock<IMapper>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.SetupGet(u => u.Orders).Returns(_mockRepository.Object);
            var mockAccountHelper = new Mock<IAccountHelper>();
            _controller = new Odin.Controllers.Api.OrdersController(mockUnitOfWork.Object, _mockMapper.Object);            
        }

        [TestMethod]
        public void UpsertDetailsServicesTest_NoServices()
        {
            var orderId = "1";
            Order order = new Order() { Id = orderId };
            _mockRepository.Setup(r => r.GetOrderById(orderId)).Returns(order);
            OrdersTransfereeDetailsServiceDto dto = new OrdersTransfereeDetailsServiceDto() { Id = "1", ScheduledDate = DateTime.Now,  CompletedDate = DateTime.Now};                
            OrdersTransfereeDetailsServicesDto dtos = new OrdersTransfereeDetailsServicesDto() { Id = "1" };
            List<OrdersTransfereeDetailsServiceDto> svc = new List<OrdersTransfereeDetailsServiceDto>();
            svc.Add(dto);
            dtos.Services= svc;
            order.Services.Add(new Service() {Id="1", ScheduledDate=DateTime.Now, CompletedDate=DateTime.Now });
            var result = _controller.UpsertDetailsServices(dtos) as IHttpActionResult;
            result.Should().BeOfType<System.Web.Http.Results.OkResult>();
        }
        [TestMethod]
        public void UpsertDetailsServicesTest_OrderNotFound()
        {
            var orderId = "1";
            Order order = new Order() { Id = orderId };
            _mockRepository.Setup(r => r.GetOrderById(orderId)).Returns(order);
            OrdersTransfereeDetailsServiceDto dto = new OrdersTransfereeDetailsServiceDto() { Id = "1", ScheduledDate = DateTime.Now, CompletedDate = DateTime.Now };
            OrdersTransfereeDetailsServicesDto dtos = new OrdersTransfereeDetailsServicesDto() { Id = "2" };
            List<OrdersTransfereeDetailsServiceDto> svc = new List<OrdersTransfereeDetailsServiceDto>();
            svc.Add(dto);
            dtos.Services = svc;
            order.Services.Add(new Service() { Id = "1", ScheduledDate = DateTime.Now, CompletedDate = DateTime.Now });
            var result = _controller.UpsertDetailsServices(dtos) as IHttpActionResult;
            result.Should().BeOfType<System.Web.Http.Results.NotFoundResult>();
        }
        [TestMethod]
        public void UpsertDetailsServicesTest_ServiceNotFound()
        {
            var orderId = "1";
            Order order = new Order() { Id = orderId };
            _mockRepository.Setup(r => r.GetOrderById(orderId)).Returns(order);
            OrdersTransfereeDetailsServiceDto dto = new OrdersTransfereeDetailsServiceDto() { Id = "1", ScheduledDate = DateTime.Now, CompletedDate = DateTime.Now };
            OrdersTransfereeDetailsServicesDto dtos = new OrdersTransfereeDetailsServicesDto() { Id = "1" };
            List<OrdersTransfereeDetailsServiceDto> svc = new List<OrdersTransfereeDetailsServiceDto>();
            svc.Add(dto);
            dtos.Services = svc;
            order.Services.Add(new Service() { Id = "2", ScheduledDate = DateTime.Now, CompletedDate = DateTime.Now });
            var result = _controller.UpsertDetailsServices(dtos) as IHttpActionResult;
            result.Should().BeOfType<System.Web.Http.Results.NotFoundResult>();
        }
    }
}
