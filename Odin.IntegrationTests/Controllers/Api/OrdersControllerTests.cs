using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Odin.IntegrationTests.Controllers.Api
{
    [TestFixture]
    public class OrdersControllerTests : WebApiBaseTest
    {
        [Test, Isolated]
        public async Task UpsertOrder_ValidRequest_ShouldNotUpdateNonDtoFields()
        {
            // Arrange
            
        }
    }
}
