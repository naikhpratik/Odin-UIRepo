using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Interfaces;
using Odin.Helpers;

namespace Odin.Tests.Helpers
{
    [TestClass]
    public class AccountHelperTests
    {
        private IAccountHelper _accountHelper;        

        [TestInitialize]
        public void TestInitialize()
        {
            var mockEmailHelper = new Mock<IEmailHelper>();
            _accountHelper = new AccountHelper(mockEmailHelper.Object);               
        }
        [TestMethod]
        public void SendEmailResetTokenAsync_DoesNotReturn_NotSent()
        {
            string _userId = "1";
            var result = _accountHelper.SendEmailResetTokenAsync(_userId);
            result.Should().NotBe("Message not sent");                
        }
        [TestMethod]
        public void SendEmailCreateTokenAsync_DoesNotReturn_NotSent()
        {
            string _userId = "1";
            var result = _accountHelper.SendEmailCreateTokenAsync(_userId);
            result.Should().NotBe("Message not sent");                
        }
    }
}