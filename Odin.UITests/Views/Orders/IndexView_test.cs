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
using Xunit;

namespace Odin.UITests.Views.Orders
{
    [TestClass]
    public class IndexView_test
    {
        private string baseURL = Globals.Url_Localhost;
       // private ApplicationStartPageTest start_page;
        private readonly IWebDriver _driver;

        public IndexView_test() {

           // start_page = new ApplicationStartPageTest();
            _driver = new ChromeDriver();
        }

       
        //checking with Pm login Add more for different logins 
        [Fact]
        public void OrdersPage_ShouldCheckForOrders()
        {
            var context = new ApplicationDbContext();
            var _unitofWork = new UnitOfWork(context);
            var Userrep = new UsersRepository(context);
            _driver.Navigate().GoToUrl(this.baseURL);
            _driver.FindElement(By.Id("Email")).SendKeys(Globals.email_pm_valid);
            _driver.FindElement(By.Id("Password")).SendKeys(Globals.pass_pm_valid);
            _driver.FindElement(By.ClassName("btn-default")).Click();
           
            //Xunit.Assert.True(start_page.Login(Globals.email_pm_valid,Globals.pass_pm_valid));
            Thread.Sleep(3000);

            //getting orders from database according to the id
            var appuser = Userrep.GetUserIdByEmail(Globals.email_pm_valid);
            
            IEnumerable<Order> order_db = _unitofWork.Orders.GetOrdersFor(appuser.Id, UserRoles.ProgramManager);
            IList<IWebElement> orders = _driver.FindElements(By.Id("rowclickableorderRow"));
            
            Xunit.Assert.Equal(orders.Count(), order_db.Count());
            
        }

        [TestCleanup]
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
            
        }

    }
}
