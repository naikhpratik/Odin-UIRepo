using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit; 
using System;
using System.Linq;

namespace Odin.UITests
{
    [TestClass]
    public class UnitTest1 : IDisposable
    {
            private string baseURL = "https://localhost:44357";
        //    private RemoteWebDriver driver;
        //    private string browser;
        //    public TestContext TestContext { get; set; }
        private readonly IWebDriver _driver;

        public UnitTest1() {
            _driver = new ChromeDriver();
        }

        

        //    //[TestMethod]
        //    public void Chrome_Login_Logout()
        //    {
        //        driver = new ChromeDriver();
        //        driver.Manage().Window.Maximize();
        //        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        //        driver.Navigate().GoToUrl(this.baseURL);

        //        Login("austin.emser@dwellworks.com", "Consultant5$");
        //        Logout();
        //    }

        [Fact]
        public void ShouldLoadApplicationPage() {
            _driver.Navigate().GoToUrl(this.baseURL);

            Xunit.Assert.Equal("Log in",_driver.Title);


        }
        //    private void Login(string username, string password)
        //    {
        //        if (driver.Url.Split('/').Last().Contains("Login"))
        //        {
        //            driver.FindElement(By.Id("Email")).SendKeys(username);
        //            driver.FindElement(By.Id("Password")).SendKeys(password);
        //            driver.FindElement(By.ClassName("btn-default")).Click();
        //        }
        //    }

        //    [TestCleanup()]
        //    public void CleanUp()
        //    {
        //        driver.Quit();
        //    }

        //    private void Logout()
        //    {
        //        driver.FindElement(By.Id("logoutForm")).Submit();
        //    }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose(); 
        }
    }
}
