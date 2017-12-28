using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using System;
using System.Linq;
using System.Threading;

namespace Odin.UITests
{
    [TestClass]
    public class ApplicationStartPage : IDisposable
    {
        private string baseURL = "https://localhost:44357";
        //    private RemoteWebDriver driver;
        //    private string browser;
        //    public TestContext TestContext { get; set; }
        private readonly IWebDriver _driver;

        public ApplicationStartPage()
        {
            _driver = new ChromeDriver();
        }

        private void gotolink()
        {
            _driver.Navigate().GoToUrl(this.baseURL);
        }

        private static void delay(int msec)
        {
            Thread.Sleep(msec);
        }

        [Fact]
        public void ShouldLoadApplicationPage_test()
        {

            gotolink();
            Xunit.Assert.Equal("Log in", _driver.Title);

        }

        [Fact]
        public void ChromeLoginLogout_test()
        {

            _driver.Manage().Window.Maximize();
            //_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            //delay(500);
            _driver.Navigate().GoToUrl(this.baseURL);

            Xunit.Assert.True(Login("Odinpm@dwellworks.com", "OdinOdin5$"));
            //delay(2000);
            Xunit.Assert.True(Logout());
            //delay(2000);

        }

        private Boolean Login(string username, string password)
        {
            if (_driver.Url.Split('/').Last().Contains("Login"))
            {
                _driver.FindElement(By.Id("Email")).SendKeys(username);
                _driver.FindElement(By.Id("Password")).SendKeys(password);
                _driver.FindElement(By.ClassName("btn-default")).Click();
                Xunit.Assert.Equal("Orders Page", _driver.Title);
                return true;
            }
            else
            {
                return false;
            }
        }
        private Boolean Logout()
        {
            _driver.FindElement(By.Id("logoutForm")).Submit();
            if (_driver.Url.Split('/').Last().Contains("Login"))
            { return true; }
            else
            { return false; }
        }

        // Can add additional functionality to verify email 
        [Fact]
        public void Forgotpasswordclick_test()
        {
            gotolink();
            _driver.FindElement(By.Id("forgot")).Click();
            var word = "";
            if (_driver.Url.Split('/').Last().Equals("ForgotPassword"))
            {
                word = "Success";
            }
            else
            {
                word = "Failure";
            }
            Xunit.Assert.Equal("Success", word);
        }

        [TestCleanup]
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
