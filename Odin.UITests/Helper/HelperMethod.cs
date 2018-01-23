using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Odin.UITests.Helper
{
    class HelperMethod
    {
        private readonly IWebDriver _driver;
        private string baseURL = Globals.Url_Localhost;
        public HelperMethod(IWebDriver _driver)
        {
            this._driver = _driver;
        }
        
        public void delay(int msec)
        {
            Thread.Sleep(msec);
        }
        
        public void initialsteps()
        {
            _driver.Navigate().GoToUrl(this.baseURL);
            _driver.Manage().Window.Maximize();
            _driver.FindElement(By.Id("Email")).SendKeys(Globals.email_pm_valid);
            _driver.FindElement(By.Id("Password")).SendKeys(Globals.pass_pm_valid);
            _driver.FindElement(By.ClassName("btn-default")).Click();

        }
        
        public IList<IWebElement> getOrders()
        {
            return _driver.FindElements(By.Id("rowclickableorderRow"));
        }

        public string GetElement(IWebDriver driver, By by, int tries)
        {

            for (int i = 1; i <= tries; i++)
            {
                try
                {
                    return driver.FindElement(by).Text;
                }
                catch (WebDriverException)
                {
                    Thread.Sleep(10);
                }
            }
            return null;

        }
        public IWebElement GetElementClick(IWebDriver driver, By by, int tries)
        {

            for (int i = 1; i <= tries; i++)
            {
                try
                {
                    return driver.FindElement(by);
                }
                catch (WebDriverException)
                {
                    Thread.Sleep(100);
                }
            }
            return null;
        }

        public void Logout()
        {
            //method_name = new stacktrace().getframe(1).getmethod().name;
            _driver.FindElement(By.Id("logoutForm")).Submit();
            _driver.Close();
        }
    }
}
