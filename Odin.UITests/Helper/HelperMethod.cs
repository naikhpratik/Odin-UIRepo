using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;

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
        public void clickWhenReady(IWebDriver driver, By locator, int timeout)
        {
            IWebElement element = null;

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));

            element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));

            element.Click();
        }

        public bool getWhenVisible(IWebDriver driver, By locator, int timeout)
        {

            var element = false;

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));

            element = wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));

            return element;

        }

        public bool closeModal(IWebDriver driver)
        {
            try
            {
                Actions action = new Actions(_driver);
                action.SendKeys(Keys.Escape).Build().Perform();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        public void Logout()
        {
            //method_name = new stacktrace().getframe(1).getmethod().name;
            _driver.FindElement(By.Id("logoutForm")).Submit();
            _driver.Close();
        }

        public bool ClickWhenReady(IWebElement property)
        {
            var flag = false;
            while (!flag)
            {
                if (property.Displayed)
                {
                    flag = true;
                }
            }
            return true;
        }

        public int GetRandomNo(IList<IWebElement> properties)
        {
            Random rnd = new Random();
            return rnd.Next(properties.Count);
        }
    }
}
