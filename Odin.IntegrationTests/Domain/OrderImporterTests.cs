using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NUnit.Framework;
using Odin.Data.Persistence;
using Odin.Domain;

namespace Odin.IntegrationTests.Domain
{
    [TestFixture]
    public class OrderImporterTests
    {
        private ApplicationDbContext context;
        private OrderImporter orderImporter;

        [SetUp]
        public void SetUp()
        {
            context = new ApplicationDbContext();

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();

            orderImporter = new OrderImporter(new UnitOfWork(context), mapper);
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }


    }
}
