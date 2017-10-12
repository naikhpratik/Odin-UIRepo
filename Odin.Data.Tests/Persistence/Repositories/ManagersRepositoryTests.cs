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
    public class ManagersRepositoryTests
    {
        private Mock<DbSet<Manager>> _mockManagers;
        private ManagersRepository _managersRepository;

        private void SetupRepositoryWithSource(IList<Manager> source)
        {
            _mockManagers = new Mock<DbSet<Manager>>();
            _mockManagers.SetSource(source);
            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.As<IApplicationDbContext>();
            mockContext.As<IApplicationDbContext>().SetupGet(c => c.Managers).Returns(_mockManagers.Object);
            _managersRepository = new ManagersRepository(mockContext.Object);
        }

        [TestMethod]
        public void addManagerTest()
        {
            var manager1 = new Manager() { Id = "manager1", Email="manager1@dwellworks.com", SeContactUid=1 };
            SetupRepositoryWithSource(new[] {manager1});
            var result = _managersRepository.GetManagerBySeContactUid(1);
            result.Should().Equals(manager1);
        }
    }
}
