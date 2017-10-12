using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using Odin.Data.Helpers;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentAssertions;
using Odin.Data.Persistence;
using Odin.Data.Tests.Extensions;

namespace Odin.Data.Tests.Persistence.Repositories
{
    [TestClass]
    public class ConsultantsRepositoryTests
    {
        private Mock<DbSet<Consultant>> _mockConsultants;
        private ConsultantsRepository _consultantsRepository;

        private void SetupRepositoryWithSource(IList<Consultant> source)
        {
            _mockConsultants = new Mock<DbSet<Consultant>>();
            _mockConsultants.SetSource(source);
            var mockContext = new Mock<ApplicationDbContext>();
            //mockContext.As<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Consultants).Returns(_mockConsultants.Object);
            _consultantsRepository = new ConsultantsRepository(mockContext.Object);
        }

        [TestMethod]
        public void addConsultantTest()
        {
            var consultant1 = new Consultant() { Id = "consultant1", Email="consultant1@dwellworks.com", SeContactUid=1 };
            SetupRepositoryWithSource(new[] {consultant1 });
            var result = _consultantsRepository.GetConsultantBySeContactUid(1);
            result.Should().Equals(consultant1);
        }
    }
}
