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

        // If transferee is old and has orders it adds order to transferee and does not add transferee
        // If transferee is new it adds the transferee
        // If Order is new it inserts the order
        // If order is old it updates the order
        // if there is a new DSC on the order it goes through a new DSC process and adds dsc to DB and order
        // if there is an old DSC on an order it goes through an email DSC process, does not add new DSC to database
        // if there order is old but the DSC is new, it goes through a DSC reassign process
        // if the order is old and the PM is new, it reassigns the Program Manager

    }
}
