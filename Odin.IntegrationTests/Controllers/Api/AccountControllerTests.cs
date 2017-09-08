using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NUnit.Framework;
using Odin.Data.Core.Models;

namespace Odin.IntegrationTests.Controllers.Api
{
    [TestFixture]
    public class AccountControllerTests : WebApiBaseTest
    {
        [Test, Isolated]
        public async Task GetUsers_ValidRequest_ShouldReturnUsers()
        {
            

        }

        
    }
}
