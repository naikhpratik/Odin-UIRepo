using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Data.Builders;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.Data.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Tests.Persistence.Repositories
{
    [TestClass]
    public class HomeFindingPropertyRepositoryTests
    {
        private Mock<DbSet<HomeFindingProperty>> _mockHomeFindingProperties;
        private HomeFindingPropertyRepository _homeFindingPropertiesRepository;

        private void SetupRepositoryWithSource(IList<HomeFindingProperty> source)
        {
            _mockHomeFindingProperties = new Mock<DbSet<HomeFindingProperty>>();
            _mockHomeFindingProperties.SetSource(source);
            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.As<IApplicationDbContext>();
            mockContext.As<IApplicationDbContext>().SetupGet(c => c.HomeFindingProperties).Returns(_mockHomeFindingProperties.Object);
            _homeFindingPropertiesRepository = new HomeFindingPropertyRepository(mockContext.Object);
        }

        [TestMethod]
        public void GetUpcomingHomeFindingPropertiesByOrderId_ReturnsOnlyFutureProperties()
        {
            HomeFinding homeFinding = HomeFindingBuilder.New();

            HomeFindingProperty yesterdayHFP = HomeFindingPropertiesBuilder.New().First();
            yesterdayHFP.ViewingDate = DateTime.Now.AddDays(-1);
            yesterdayHFP.HomeFinding = homeFinding;

            HomeFindingProperty tomorrowHFP = HomeFindingPropertiesBuilder.New().First();
            tomorrowHFP.ViewingDate = DateTime.Now.AddDays(1);
            tomorrowHFP.HomeFinding = homeFinding;

            HomeFindingProperty nextWeekHFP = HomeFindingPropertiesBuilder.New().First();
            nextWeekHFP.ViewingDate = DateTime.Now.AddDays(7);
            nextWeekHFP.HomeFinding = homeFinding;

            HomeFindingProperty noShowingHFP = HomeFindingPropertiesBuilder.New().First();
            noShowingHFP.ViewingDate = null;
            noShowingHFP.HomeFinding = homeFinding;

            var hfps = new[] { yesterdayHFP, tomorrowHFP, nextWeekHFP, noShowingHFP };
            SetupRepositoryWithSource(hfps);
            
            // Act
            IEnumerable<HomeFindingProperty> upcomingViewings = _homeFindingPropertiesRepository.GetUpcomingHomeFindingPropertiesByHomeFindingId(homeFinding.Id);

            // Assert
            upcomingViewings.Count().Should().Be(2);
            upcomingViewings.Should().NotContain(yesterdayHFP);
            upcomingViewings.Should().NotContain(noShowingHFP);
            upcomingViewings.Should().Contain(tomorrowHFP);
            upcomingViewings.Should().Contain(nextWeekHFP);
        }
    }
}