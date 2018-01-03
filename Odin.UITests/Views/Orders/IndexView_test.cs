using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using Xunit;
using Xunit.Abstractions;

namespace Odin.UITests.Views.Orders
{
    [TestClass]
    public class IndexView_test
    {
        private string baseURL = Globals.Url_Localhost;
        // private ApplicationStartPageTest start_page;
        private readonly IWebDriver _driver;
        private ApplicationDbContext _context;
        private UnitOfWork _unitOfWork;
        private UsersRepository userRepo;
        private readonly ITestOutputHelper output;
        public IEnumerable<Order> orderFrom_db;

        public IndexView_test()
        {

            // start_page = new ApplicationStartPageTest();
            _driver = new ChromeDriver();
            // _driver = new PhantomJSDriver();
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
            userRepo = new UsersRepository(_context);
            //orderFrom_db = new IEnumerable<Order>();
        }

        private void initialsteps()
        {

            _driver.Navigate().GoToUrl(this.baseURL);
            _driver.Manage().Window.Maximize();
            _driver.FindElement(By.Id("Email")).SendKeys(Globals.email_pm_valid);
            _driver.FindElement(By.Id("Password")).SendKeys(Globals.pass_pm_valid);
            _driver.FindElement(By.ClassName("btn-default")).Click();

        }
        //checking with Pm login Add more for different logins 
        [Fact]
        public void OrdersPage_ShouldCheckForOrders()
        {

            initialsteps();

            //Thread.Sleep(3000);

            //getting orders from database according to the id
            var appuser = userRepo.GetUserIdByEmail(Globals.email_pm_valid);

            IEnumerable<Order> order_db = _unitOfWork.Orders.GetOrdersFor(appuser.Id, UserRoles.ProgramManager);
            IList<IWebElement> orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            //Thread.Sleep(3000);
            Xunit.Assert.Equal(orders.Count(), order_db.Count());

            Logout();
            //Thread.Sleep(3000);
        }

        [Fact]
        public void OrdersPage_VerifyAllOrders_Displayed()
        {

            initialsteps();

            var appuser = userRepo.GetUserIdByEmail(Globals.email_pm_valid);

            orderFrom_db = _unitOfWork.Orders.GetOrdersFor(appuser.Id, UserRoles.ProgramManager);
            IList<IWebElement> orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            //Create Dictionary to match the order by their Full Name`s from DB and UI

            Xunit.Assert.True(match_Orders(orderFrom_db, orders));
            Logout();

        }

        [Fact]
        public void OrdersPage_VerifyAllOrdersbyClicking_ShouldReturnTrue()
        {

            initialsteps();

            var appuser = userRepo.GetUserIdByEmail(Globals.email_pm_valid);
            //orderFrom_db = _unitOfWork.Orders.GetOrdersFor(appuser.Id, UserRoles.ProgramManager);
            IList<IWebElement> orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            for (int i = 0; i < orders.Count(); i++)
            {
                var index_Name = orders.ElementAt(i).Text.Substring(0, orders.ElementAt(i).Text.IndexOf("\r"));
                orders.ElementAt(i).Click();
                var transferee_Name = _driver.FindElement(By.ClassName("eeName")).Text;
                _driver.FindElement(By.Id("backButton")).Click();

                Xunit.Assert.Equal(index_Name, transferee_Name);

                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }


            //Xunit.Assert.True(true);

            Logout();

        }

        [Fact]
        public void OrdersPage_VerifyTransfereePageElements_Displayed()
        {

            initialsteps();

            var appuser = userRepo.GetUserIdByEmail(Globals.email_pm_valid);

            //orderFrom_db = _unitOfWork.Orders.GetOrdersFor(appuser.Id, UserRoles.ProgramManager);
            IList<IWebElement> orders = _driver.FindElements(By.Id("rowclickableorderRow"));


            for (int i = 0; i < orders.Count(); i++)
            {

                var order_Id = orders.ElementAt(i).GetAttribute("data-order-id");
                _driver.Navigate().GoToUrl(this.baseURL+ "/Orders/Transferee/"+order_Id);

                //To click on the elements rather then wait for the refresh or using delay
                IJavaScriptExecutor executor = (IJavaScriptExecutor)_driver;
                executor.ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("intake")));
                executor.ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("details")));
                executor.ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("housing")));
                executor.ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("history")));
                executor.ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("messages")));
                executor.ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("itinerary")));
                
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders");
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }

            //Xunit.Assert.True(match_Orders(orderFrom_db, orders));
            Logout();

        }





        private void delay(int msec)
        {
            Thread.Sleep(msec);
        }

        private bool match_Orders(IEnumerable<Order> orderFrom_db, IList<IWebElement> orders)
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

        private void Logout()
        {
            _driver.FindElement(By.Id("logoutForm")).Submit();
        }

        [TestCleanup]
        private void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();

        }

    }
}
