using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Interfaces;
using Odin.Helpers;

namespace Odin.Tests.Helpers
{
    [TestClass]
    class AccountHelperTests
    {
            private IAccountHelper _accountHelper;
                   
            [TestMethod]
            public void Index_WhenCalled_ReturnsOk()
            {
                var mockEmailHelper = new Mock<IEmailHelper>();  
                _accountHelper = new AccountHelper(mockEmailHelper.Object);
                string _userId = "1";
                var result = _accountHelper.SendEmailCreateTokenAsync(_userId);
                result.Should().NotBe("Message not sent");
            }
        }
    }