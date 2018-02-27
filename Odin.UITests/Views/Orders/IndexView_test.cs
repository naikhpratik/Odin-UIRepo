using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.UITests.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using Xunit;


namespace Odin.UITests.Views.Orders
{
    //[Collection("Our Test Collection #1")]
    [TestClass]
    public class IndexView_test
    {

        private readonly IWebDriver _driver;
        private ApplicationDbContext _context;
        private UnitOfWork _unitOfWork;
        private UsersRepository userRepo;
        public IEnumerable<Order> orderFrom_db;
        //private string method_Name;
        private HelperMethod help;
        private string baseURL;

        public IndexView_test()
        {
            
            //_driver = new ChromeDriver();
            _driver = new PhantomJSDriver();
            help = new HelperMethod(_driver);
           
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
            userRepo = new UsersRepository(_context);
            baseURL = Globals.Url_Localhost;
        }

        //checking with Pm login Add more for different logins 
        [TestMethod]
        public void OrdersPage_ShouldCheckForOrders()
        {

            help.initialsteps();

            //getting orders from database according to the id
            var appuser = userRepo.GetUserIdByEmail(Globals.email_pm_valid);
            IList<IWebElement> orders = null;
            IEnumerable<Order> order_db = _unitOfWork.Orders.GetOrdersFor(appuser.Id, UserRoles.ProgramManager);
            //if (help.getWhenVisible(_driver, By.Id("rowclickableorderRow"), 3)) {
                 orders = _driver.FindElements(By.Id("rowclickableorderRow"));
            //}

            Xunit.Assert.Equal(orders.Count(), order_db.Count());

            help.Logout();

        }

        [TestMethod]
        public void OrdersPage_VerifyAllOrders_Displayed()
        {
            help.initialsteps();

            var appuser = userRepo.GetUserIdByEmail(Globals.email_pm_valid);

            orderFrom_db = _unitOfWork.Orders.GetOrdersFor(appuser.Id, UserRoles.ProgramManager);
            IList<IWebElement> orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            //Create Dictionary to match the order by their Full Name`s from DB and UI

            Xunit.Assert.True(help.match_Orders(orderFrom_db, orders));
            help.Logout();

        }

        [TestMethod]
        public void OrdersPage_VerifyAllOrdersbyClicking_ShouldReturnTrue()
        {

            help.initialsteps();

            var appuser = userRepo.GetUserIdByEmail(Globals.email_pm_valid);
            //orderFrom_db = _unitOfWork.Orders.GetOrdersFor(appuser.Id, UserRoles.ProgramManager);
            IList<IWebElement> orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            for (int i = 0; i < orders.Count(); i++)
            {
                if (i == 3) break;
                var random = help.GetRandomNo(orders);
                
                var index_Name = orders.ElementAt(random).Text.Substring(0, orders.ElementAt(random).Text.IndexOf("\r"));
                orders.ElementAt(random).Click();
                var transferee_Name = _driver.FindElement(By.ClassName("eeName")).Text;

                Xunit.Assert.Equal(index_Name, transferee_Name);
                
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders");
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));
            }
            
            help.Logout();

        }

        [TestMethod]
        public void OrdersPage_VerifyTransfereePageElements_Displayed()
        {

            help.initialsteps();

            var appuser = userRepo.GetUserIdByEmail(Globals.email_pm_valid);

            //orderFrom_db = _unitOfWork.Orders.GetOrdersFor(appuser.Id, UserRoles.ProgramManager);
            IList<IWebElement> orders = _driver.FindElements(By.Id("rowclickableorderRow"));


            for (int i = 0; i < orders.Count(); i++)
            {
                if (i == 3) break;
                
                var order_Id = orders.ElementAt(help.GetRandomNo(orders)).GetAttribute("data-order-id");
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_Id);

                //To click on the elements rather then wait for the refresh or using delay
                IJavaScriptExecutor executor = (IJavaScriptExecutor)_driver;
                executor.ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("intake")));
                executor.ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("details")));
                executor.ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("housing")));
                executor.ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("history")));
                // executor.ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("messages")));
                executor.ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("itinerary")));

                _driver.Navigate().GoToUrl(this.baseURL + "/Orders");
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }

            //Xunit.Assert.True(match_Orders(orderFrom_db, orders));
            help.Logout();

        }

        [TestMethod]
        public void OrdersIndexPage_SwitchDifferntPM_NavigateViaURL_ShouldDisplayRespectiveOrders()
        {

            help.initialsteps();
            
            IList<IWebElement> program_managers = _driver.FindElements(By.ClassName("clickablepm"));
            
            for (int i = 0; i < program_managers.Count(); i++)
            {
                if (i == 3) break;
                //getting PM id
                var pm_Id = program_managers.ElementAt(help.GetRandomNo(program_managers)).GetAttribute("data-order-id");

                //Navigating by URL
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Index/" + pm_Id);
                help.delay(500);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders");
                help.delay(500);
                program_managers = _driver.FindElements(By.ClassName("clickablepm"));

            }

            //Xunit.Assert.True(match_Orders(orderFrom_db, orders));
            help.Logout();

        }

        [TestMethod]
        public void OrdersIndexPage_SwitchDifferntPM_Onclick_ShouldDisplayRespectiveOrders()
        {

            help.initialsteps();
            
            IList<IWebElement> program_managers = _driver.FindElements(By.ClassName("clickablepm"));


            for (int i = 0; i < program_managers.Count(); i++)
            {
                //getting PM id
                //var pm_Id = program_managers.ElementAt(i).GetAttribute("data-order-id");
                if (i == 3) break;

                var random = help.GetRandomNo(program_managers);
                //getting PM id
                var pm_Id = program_managers.ElementAt(random).GetAttribute("data-order-id");

                orderFrom_db = _unitOfWork.Orders.GetOrdersFor(pm_Id, UserRoles.ProgramManager);

                //Navigating by clicking
                //To click on the elements rather then wait for the refresh or using delay
                IJavaScriptExecutor executor = (IJavaScriptExecutor)_driver;
                executor.ExecuteScript("arguments[0].click();", _driver.FindElement(By.Id("dropdownMenuLink")));
                executor.ExecuteScript("arguments[0].click();", program_managers.ElementAt(random));
                help.delay(500);
                IList<IWebElement> orders = _driver.FindElements(By.Id("rowclickableorderRow"));

                Xunit.Assert.True(help.match_Orders(orderFrom_db, orders));

                //_driver.Navigate().GoToUrl(this.baseURL + "/Orders");
                //delay(3000);
                program_managers = _driver.FindElements(By.ClassName("clickablepm"));

            }

            //Xunit.Assert.True(match_Orders(orderFrom_db, orders));
            help.Logout();

        }

        [TestCleanup]
        public void Dispose()
        {

            //System.Diagnostics.Debug.WriteLine("Calling Method : " + method_Name);
            _driver.Quit();
            _driver.Dispose();
        }

    }
}
