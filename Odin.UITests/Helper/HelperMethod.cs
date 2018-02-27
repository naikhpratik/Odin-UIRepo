using Odin.Data.Core.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public int GetRandomNo(IList<IWebElement> elements)
        {
            Random rnd = new Random();
            return rnd.Next(elements.Count);
        }

        public bool match_Orders(IEnumerable<Order> orderFrom_db, IList<IWebElement> orders)
        {
            //Create Dictionary to match the order by their Full Name`s from DB and UI

            Dictionary<string, int> verify;
            if (orderFrom_db.Count() == orders.Count())
            {
                verify = new Dictionary<string, int>();

                for (int i = 0; i < orderFrom_db.Count(); i++)
                {
                    var Db_firstname = orderFrom_db.ElementAt(i).Transferee.FullName;

                    var UI_firstname = orders.ElementAt(i).Text.Substring(0, orders.ElementAt(i).Text.IndexOf("\r"));

                    if (!Db_firstname.Equals(UI_firstname))
                    {
                        if (verify.ContainsKey(Db_firstname))
                        {
                            if (verify[Db_firstname] > 1)
                                verify.Add(Db_firstname, verify[Db_firstname] - 1);
                            else
                                verify.Remove(Db_firstname);
                        }
                        else
                        {
                            verify.Add(Db_firstname, 1);
                        }
                        if (verify.ContainsKey(UI_firstname))
                        {
                            if (verify[UI_firstname] > 1)
                                verify.Add(UI_firstname, verify[UI_firstname] - 1);
                            else
                                verify.Remove(UI_firstname);
                        }
                        else
                        {
                            verify.Add(UI_firstname, 1);
                        }
                    }
                }
                return verify.Count() > 0 ? false : true;
            }

            return false;

        }

    }
}
