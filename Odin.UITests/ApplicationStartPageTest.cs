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
    public class ApplicationStartPageTest : IDisposable
    {
        private string baseURL = Globals.Url_Localhost;
        private readonly IWebDriver _driver;
        //    private RemoteWebDriver driver;
        //    private string browser;
        //    public TestContext TestContext { get; set; }

        public ApplicationStartPageTest(){
            _driver = new ChromeDriver();
        }

        public void Gotolink()
        {
            _driver.Navigate().GoToUrl(this.baseURL);
        }

        public void Delay(int msec)
        {
            Thread.Sleep(msec);
        }

        [Fact]
        public void ShouldLoadApplicationPage_test()
        {

            Gotolink();
            Xunit.Assert.Equal("Log in", _driver.Title);

        }

        [Fact]
        public void ChromeLoginLogout_test()
        {

            _driver.Manage().Window.Maximize();
            //_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            //delay(500);
            _driver.Navigate().GoToUrl(this.baseURL);

            Xunit.Assert.True(Login(Globals.email_pm_valid,Globals.pass_pm_valid));
            //delay(2000);
            Xunit.Assert.True(Logout());
            //delay(2000);

        }

        public Boolean Login(string username, string password)
        {
            if (_driver.Url.Split('/').Last().Contains("Login"))
            {
                _driver.FindElement(By.Id("Email")).SendKeys(username);
                _driver.FindElement(By.Id("Password")).SendKeys(password);
                _driver.FindElement(By.ClassName("btn-default")).Click();
                if(_driver.Url.Split('/').Last().Contains("Orders"))
                    Xunit.Assert.Equal("Orders Page", _driver.Title);
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Logout()
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
            Gotolink();
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

        [Fact]
        public void LoginPage_InvalidCredentails_ShouldCheckForErrors()
        {
            Gotolink();
            Xunit.Assert.True(Login("", ""));
            Xunit.Assert.Equal("The Email field is required.", _driver.FindElement(By.Id("validation_email")).Text);
            Xunit.Assert.Equal("The Password field is required.", _driver.FindElement(By.Id("validation_password")).Text);
        }
        
        [TestCleanup]
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
