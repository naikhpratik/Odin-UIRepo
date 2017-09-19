using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Data.Core.Repositories;
using Odin.Domain;

namespace Odin.Tests.Domain
{
    [TestClass]
    public class OrderImporterTests
    {
        private OrderImporter _orderImporter;
        private Mock<IOrdersRepository> _mockOrdersRepository;
        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void TestInitialize()
        {
            
        }
    }
}
