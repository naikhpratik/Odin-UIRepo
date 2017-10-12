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
    public class TransfereesRepositoryTests
    {
        private Mock<DbSet<Transferee>> _mockTransferees;
        private TransfereesRepository _transfereesRepository;

        private void SetupRepositoryWithSource(IList<Transferee> source)
        {
            _mockTransferees = new Mock<DbSet<Transferee>>();
            _mockTransferees.SetSource(source);
            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.As<IApplicationDbContext>();
            mockContext.As<IApplicationDbContext>().SetupGet(c => c.Transferees).Returns(_mockTransferees.Object);            
            _transfereesRepository = new TransfereesRepository(mockContext.Object);
        }

        [TestMethod]
        public void addTransfereeTest()
        {
            var transferee1 = new Transferee() { Id = "Transferee1", Email="transferee1@dwellworks.com", SeContactUid=1 };
            SetupRepositoryWithSource(new[] {transferee1 });
            var result = _transfereesRepository.GetTransfereeByEmail("transferee1@dwellworks.com");
            result.Should().Equals(transferee1);
        }
    }
}
