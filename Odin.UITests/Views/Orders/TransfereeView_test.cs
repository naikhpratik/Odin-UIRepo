using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Odin.UITests.Views.Orders
{
    public class TransfereeView_test
    {
        private string baseURL = Globals.Url_Localhost;
        private readonly IWebDriver _driver;
        private ApplicationDbContext _context;
        private UnitOfWork _unitOfWork;
        private UsersRepository userRepo;
        public IEnumerable<Order> orderFrom_db;
        private IList<IWebElement> orders;
        private object method_Name;

        public TransfereeView_test()
        {
            //DesiredCapabilities capabilities = DesiredCapabilities.Chrome();
            //ChromeOptions options = new ChromeOptions();
            //options.AddArguments("--incognito");
            //options.ToCapabilities();

            _driver = new ChromeDriver();
            //_driver = new PhantomJSDriver();
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
            userRepo = new UsersRepository(_context);

        }

        private void initialsteps()
        {
            _driver.Navigate().GoToUrl(this.baseURL);
            _driver.Manage().Window.Maximize();
            _driver.FindElement(By.Id("Email")).SendKeys(Globals.email_pm_valid);
            _driver.FindElement(By.Id("Password")).SendKeys(Globals.pass_pm_valid);
            _driver.FindElement(By.ClassName("btn-default")).Click();
            orders = _driver.FindElements(By.Id("rowclickableorderRow"));
            
        }

        [Fact]
        public void Transferee_IntakePage_ShouldCheckForTransfereeDetails()
        {

            initialsteps();

            //getting orders from database according to the id
            //var appuser = userRepo.GetUserIdByEmail(Globals.email_pm_valid);
            //IEnumerable<Order> order_db = _unitOfWork.Orders.GetOrdersFor(appuser.Id, UserRoles.ProgramManager);

            //delay(3000);

            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id); ;
                orders.ElementAt(i).Click();

                //Check for contact Info

                delay(50);
                Xunit.Assert.Equal(db_order.Transferee.FullName, _driver.FindElement(By.Id("Transferee_FullName")).Text);
                Xunit.Assert.Equal(db_order.Transferee.Email, _driver.FindElement(By.Id("Transferee_Email")).Text);
                Xunit.Assert.Equal(db_order.Transferee.PhoneNumber, _driver.FindElement(By.Id("Transferee_PhoneNumber")).Text);

                //Check for Rmc Info

                Xunit.Assert.Equal(db_order.Rmc, _driver.FindElement(By.Id("Rmc")).Text);

                //*** Check what all information is important or Not null attributes **** //

                //var rmc1 = _driver.FindElement(By.Id("Rmc_contact")).Text;
                //Xunit.Assert.Equal(db_order.RmcContact,rmc1);
                //Xunit.Assert.Equal(db_order.RmcContactEmail, _driver.FindElement(By.Id("Rmc_contactemail")).Text);

                _driver.FindElement(By.Id("backButton")).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }


            // Xunit.Assert.Equal(orders.Count(), order_db.Count());

            Logout();

        }

        [Fact]
        public void Transferee_Intakepage_ShouldCheckandUpdateDestinationLocation()
        {

            initialsteps();

            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id); ;
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id);
                //orders.ElementAt(i).Click();

                //Check for contact Info

                delay(50);
                
                //var city = _driver.FindElement(By.CssSelector("span#spanDestinationCity.intake-span")).Text;
                //var state = _driver.FindElement(By.Id("spanDestinationState")).Text;
                //var country = _driver.FindElement(By.Id("spanDestinationCountry")).Text;
                Xunit.Assert.Equal(db_order.DestinationCity,  _driver.FindElement(By.CssSelector("span#spanDestinationCity.intake-span")).Text);
                Xunit.Assert.Equal(db_order.DestinationState,  _driver.FindElement(By.CssSelector("span#spanDestinationState.intake-span")).Text);
                Xunit.Assert.Equal(db_order.DestinationCountry, _driver.FindElement(By.CssSelector("span#spanDestinationCountry.intake-span")).Text);

                _driver.FindElement(By.XPath("//span[@class = 'sectionSave intake-edit'][1]")).Click();

                _driver.FindElement(By.XPath("//span[@class = 'sectionSave intake-edit'][1]")).Click();
                //Xunit.Assert.Equal(db_order.Rmc, _driver.FindElement(By.Id("Rmc")).Text);

                //*** Check what all information is important or Not null attributes **** //

                //var rmc1 = _driver.FindElement(By.Id("Rmc_contact")).Text;
                //Xunit.Assert.Equal(db_order.RmcContact,rmc1);
                //Xunit.Assert.Equal(db_order.RmcContactEmail, _driver.FindElement(By.Id("Rmc_contactemail")).Text);

                _driver.FindElement(By.Id("backButton")).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }


            // Xunit.Assert.Equal(orders.Count(), order_db.Count());

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
            method_Name = new StackTrace().GetFrame(1).GetMethod().Name;
            _driver.FindElement(By.Id("logoutForm")).Submit();
            _driver.Close();
        }

        [TestCleanup]
        private void Dispose()
        {

            System.Diagnostics.Debug.WriteLine("Calling Method : " + method_Name);
            _driver.Quit();
            _driver.Dispose();


        }
    }
}
