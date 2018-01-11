using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.Helpers;
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
    [TestClass]
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


                delay(800);
                //Check for contact Info


                Xunit.Assert.Equal(db_order.Transferee.FullName, _driver.FindElement(By.Id("Transferee_FullName")).Text);
                Xunit.Assert.Equal(db_order.Transferee.Email, _driver.FindElement(By.Id("Transferee_Email")).Text);
                //Xunit.Assert.Equal(db_order.Transferee.PhoneNumber.Replace("-",""), _driver.FindElement(By.Id("Transferee_PhoneNumber")).Text.Replace(".",""));

                //Check for Rmc Info

                //Xunit.Assert.Equal(db_order.Rmc, _driver.FindElement(By.Id("Rmc")).Text);

                //*** Check what all information is important or Not null attributes **** //

                //var rmc1 = _driver.FindElement(By.Id("Rmc_contact")).Text;
                //Xunit.Assert.Equal(db_order.RmcContact,rmc1);
                //Xunit.Assert.Equal(db_order.RmcContactEmail, _driver.FindElement(By.Id("Rmc_contactemail")).Text);

                // _driver.FindElement(By.Id("backButton")).Click();
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders");
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
                delay(800);
                //Check for Destiantion and Departure Info

                /**
                //var city = _driver.FindElement(By.CssSelector("span#spanDestinationCity.intake-span")).Text;
                //var state = _driver.FindElement(By.Id("spanDestinationState")).Text;
                //var country = _driver.FindElement(By.Id("spanDestinationCountry")).Text;
                // Xunit.Assert.Equal(db_order.DestinationCity, _driver.FindElement(By.CssSelector("span#spanDestinationCity.intake-span")).Text);
                // Xunit.Assert.Equal(db_order.DestinationState, _driver.FindElement(By.CssSelector("span#spanDestinationState.intake-span")).Text);
                // Xunit.Assert.Equal(db_order.DestinationCountry, _driver.FindElement(By.CssSelector("span#spanDestinationCountry.intake-span")).Text);
                **/
                var click_expand = GetElementClick(_driver, By.XPath("(//img[@class = 'intake-expand-img'])[5]"), 10);
                var click_collapse = GetElementClick(_driver, By.XPath("(//img[@class = 'intake-collapse-img'])[5]"), 10);
                ((IJavaScriptExecutor)_driver).ExecuteScript("scroll(0,400)");
                click_expand.Click();
                click_collapse.Click();
                //delay(100);
                click_expand.Click();


                //** Destination Location **//
                Xunit.Assert.Equal(db_order.DestinationCity, GetElement(_driver, By.Id("spanDestinationCity"), 10));
                Xunit.Assert.Equal(db_order.DestinationState, _driver.FindElement(By.Id("spanDestinationState")).Text);
                Xunit.Assert.Equal(db_order.DestinationCountry, _driver.FindElement(By.Id("spanDestinationCountry")).Text);

                _driver.FindElement(By.XPath("(//span[@class = 'sectionSave intake-edit'])[3]")).Click();
                _driver.FindElement(By.XPath("(//span[@class = 'sectionSave intake-cancel'])[3]")).Click();
                _driver.FindElement(By.XPath("(//span[@class = 'sectionSave intake-edit'])[3]")).Click();
                _driver.FindElement(By.Id("DestinationCity")).SendKeys("Test City");
                _driver.FindElement(By.Id("DestinationState")).SendKeys("Test State");
                _driver.FindElement(By.Id("DestinationCountry")).SendKeys("Test Country");

                _driver.FindElement(By.XPath("(//span[@class = 'sectionSave intake-edit'])[3]")).Click();

                /****** Check the saved values once the code is Uptodate ******/

                //check for the update with the database 

                /****** End ******/

                _driver.Navigate().GoToUrl(this.baseURL + "/Orders");
                //_driver.FindElement(By.Id("backButton")).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }


            // Xunit.Assert.Equal(orders.Count(), order_db.Count());

            Logout();

        }

        [Fact]
        public void Transferee_Intakepage_ShouldCheckandUpdateDepartureLocation()
        {

            initialsteps();

            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id);
                delay(800);
                //Check for Destiantion and Departure Info

                /**
                var city = GetElement( _driver, By.Id("spanOriginCity"),10);
                var state = _driver.FindElement(By.Id("spanOriginState")).Text;
                var country = _driver.FindElement(By.Id("spanOriginCountry")).Text;
                Xunit.Assert.Equal(db_order.DestinationCity, _driver.FindElement(By.CssSelector("span#spanDestinationCity.intake-span")).Text);
                Xunit.Assert.Equal(db_order.DestinationState, _driver.FindElement(By.CssSelector("span#spanDestinationState.intake-span")).Text);
                Xunit.Assert.Equal(db_order.DestinationCountry, _driver.FindElement(By.CssSelector("span#spanDestinationCountry.intake-span")).Text);
                **/

                //** Departure Location **//

                //Xunit.Assert.Equal(db_order.DestinationCity, _driver.FindElement(By.Id("spanDestinationCity")).Text);

                var click_expand = GetElementClick(_driver, By.XPath("(//img[@class = 'intake-expand-img'])[6]"), 10);
                var click_collapse = GetElementClick(_driver, By.XPath("(//img[@class = 'intake-collapse-img'])[6]"), 10);
                ((IJavaScriptExecutor)_driver).ExecuteScript("scroll(0,400)");
                click_expand.Click();
                click_collapse.Click();
                //delay(100);
                click_expand.Click();


                Xunit.Assert.Equal(db_order.OriginCity, GetElement(_driver, By.Id("spanOriginCity"), 10));
                Xunit.Assert.Equal(db_order.OriginState, _driver.FindElement(By.Id("spanOriginState")).Text);
                Xunit.Assert.Equal(db_order.OriginCountry, _driver.FindElement(By.Id("spanOriginCountry")).Text);

                _driver.FindElement(By.XPath("(//span[@class = 'sectionSave intake-edit'])[4]")).Click();
                _driver.FindElement(By.XPath("(//span[@class = 'sectionSave intake-cancel'])[4]")).Click();
                _driver.FindElement(By.XPath("(//span[@class = 'sectionSave intake-edit'])[4]")).Click();
                _driver.FindElement(By.Id("OriginCity")).SendKeys("Test City");
                _driver.FindElement(By.Id("OriginState")).SendKeys("Test State");
                _driver.FindElement(By.Id("OriginCountry")).SendKeys("Test Country");
                _driver.FindElement(By.XPath("(//span[@class = 'sectionSave intake-edit'])[4]")).Click();

                /****** Check the saved values once the code is Uptodate ******/

                //check for the update with the database 

                /****** End ******/

                //_driver.FindElement(By.Id("backButton")).Click();
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders");
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }


            // Xunit.Assert.Equal(orders.Count(), order_db.Count());

            Logout();

        }

        [Fact]
        public void Transferee_Intakepage_ShouldCheckandUpdateRelocationDates()
        {

            initialsteps();

            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id); ;
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id);

                //Check for Destiantion and Departure Info

                /**
                var city = GetElement( _driver, By.Id("spanOriginCity"),10);
                var state = _driver.FindElement(By.Id("spanOriginState")).Text;
                var country = _driver.FindElement(By.Id("spanOriginCountry")).Text;
                var prepre = DateHelper.GetViewFormat(db_order.PreTripDate);
                var Pre = GetElement(_driver, By.Id("spanPreTripDateDisplay"), 10);
                Xunit.Assert.Equal(db_order.DestinationCity, _driver.FindElement(By.CssSelector("span#spanDestinationCity.intake-span")).Text);
                Xunit.Assert.Equal(db_order.DestinationState, _driver.FindElement(By.CssSelector("span#spanDestinationState.intake-span")).Text);
                Xunit.Assert.Equal(db_order.DestinationCountry, _driver.FindElement(By.CssSelector("span#spanDestinationCountry.intake-span")).Text);
                **/

                //** Departure Location **//

                delay(800);
                Xunit.Assert.Equal(DateHelper.GetViewFormat(db_order.PreTripDate), GetElement(_driver, By.Id("spanPreTripDate"), 10));
                Xunit.Assert.Equal(DateHelper.GetViewFormat(db_order.EstimatedArrivalDate), GetElement(_driver, By.Id("spanEstimatedArrivalDate"), 10));
                Xunit.Assert.Equal(DateHelper.GetViewFormat(db_order.WorkStartDate), _driver.FindElement(By.Id("spanWorkStartDate")).Text);

                //Complete//

                /****** Check the saved values once the code is Uptodate ******/

                //check for the update with the database 

                /****** End ******/
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders");
                //_driver.FindElement(By.Id("backButton")).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }


            // Xunit.Assert.Equal(orders.Count(), order_db.Count());

            Logout();

        }

        [Fact]
        public void Transferee_Intakepage_ShouldCheckandUpdateGeneral()
        {

            initialsteps();

            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id);
                delay(800);


                /**
                var city = GetElement( _driver, By.Id("spanOriginCity"),10);
                var state = _driver.FindElement(By.Id("spanOriginState")).Text;
                var country = _driver.FindElement(By.Id("spanOriginCountry")).Text;
                var prepre = DateHelper.GetViewFormat(db_order.PreTripDate);
                var Pre = GetElement(_driver, By.Id("spanPreTripDateDisplay"), 10);
                Xunit.Assert.Equal(db_order.DestinationCity, _driver.FindElement(By.CssSelector("span#spanDestinationCity.intake-span")).Text);
                Xunit.Assert.Equal(db_order.DestinationState, _driver.FindElement(By.CssSelector("span#spanDestinationState.intake-span")).Text);
                Xunit.Assert.Equal(db_order.DestinationCountry, _driver.FindElement(By.CssSelector("span#spanDestinationCountry.intake-span")).Text);
                **/


                var click_expand = GetElementClick(_driver, By.XPath("(//img[@class = 'intake-expand-img'])[11]"), 10);
                var click_collapse = GetElementClick(_driver, By.XPath("(//img[@class = 'intake-collapse-img'])[11]"), 10);
                ((IJavaScriptExecutor)_driver).ExecuteScript("scroll(0,1200)");
                click_expand.Click();
                click_collapse.Click();
                //delay(1000);
                click_expand.Click();

                delay(100);
                //Xunit.Assert.Equal(DateHelper.GetViewFormat(db_order.IsRush), GetElement(_driver, By.Id("spanRush"), 10));

                if (db_order.IsRush)
                {
                    Xunit.Assert.Equal("Yes", GetElement(_driver, By.Id("spanRush"), 10));
                }
                else
                    Xunit.Assert.Equal("No", GetElement(_driver, By.Id("spanRush"), 10));

                if (db_order.IsVip)
                {
                    Xunit.Assert.Equal("Yes", GetElement(_driver, By.Id("spanVip"), 10));
                }
                else
                    Xunit.Assert.Equal("No", GetElement(_driver, By.Id("spanVip"), 10));

                if (db_order.IsAssignment)
                {
                    Xunit.Assert.Equal("Temporary", GetElement(_driver, By.Id("spanAssignment"), 10));
                }
                else
                    Xunit.Assert.Equal("Permanent", GetElement(_driver, By.Id("spanAssignment"), 10));

                if (db_order.IsInternational)
                {
                    Xunit.Assert.Equal("International", GetElement(_driver, By.Id("spanInternational"), 10));
                }
                else
                    Xunit.Assert.Equal("Domestic", GetElement(_driver, By.Id("spanInternational"), 10));


                //Xunit.Assert.Equal(DateHelper.GetViewFormat(db_order.EstimatedArrivalDate), GetElement(_driver, By.Id("spanVip"), 10));
                //Xunit.Assert.Equal(DateHelper.GetViewFormat(db_order.WorkStartDate), _driver.FindElement(By.Id("spanAssignment")).Text);
                //Xunit.Assert.Equal(DateHelper.GetViewFormat(db_order.WorkStartDate), _driver.FindElement(By.Id("spanInternational")).Text);
                //Complete//

                /****** Check the saved values once the code is Uptodate ******/

                //check for the update with the database 

                /****** End ******/

                //_driver.FindElement(By.Id("backButton")).Click();
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders");
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }


            // Xunit.Assert.Equal(orders.Count(), order_db.Count());

            Logout();

        }

        //check after merge
        [Fact]
        public void Transferee_Intakepage_ShouldCheckServicesonDetailspage()
        {

            initialsteps();

            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id);
                delay(800);

                var selectedservices_count = db_order.Services.Count(x => x.Selected == true);

                IList<IWebElement> intake_services = _driver.FindElements(By.XPath("//div[@data-entity-collection = 'services']"));

                Xunit.Assert.Equal(intake_services.Count(), db_order.Services.Count());


                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#details");
                delay(800);
                IList<IWebElement> details_services = _driver.FindElements(By.XPath("//ul[@data-entity-collection = 'services']"));

                Xunit.Assert.Equal(selectedservices_count, details_services.Count());

                _driver.FindElement(By.Id("backButton")).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }

            Logout();

        }


        [Fact]
        [Ignore]
        public void Transferee_Detailspage_ShouldCheckprofilesummary()
        {
            //Due to recent changes this test is No more 
            //initialsteps();

            //for (int i = 0; i < orders.Count(); i++)
            //{

            //    var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
            //    var db_order = _unitOfWork.Orders.GetOrderById(order_id);
            //    _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#details");

            //    delay(800);

            //    // File Details

            //    Xunit.Assert.Equal(db_order.ProgramManager.FullName, _driver.FindElement(By.Id("pmFname")).Text);
            //    Xunit.Assert.Equal(db_order.ProgramManager.Email, _driver.FindElement(By.Id("pmemail")).Text);
            //    Xunit.Assert.Equal(db_order.ProgramManager.PhoneNumber, _driver.FindElement(By.Id("pmpno")).Text.Replace(".", ""));

            //    // Important Dates

            //    // Housing details

            //    _driver.FindElement(By.Id("backButton")).Click();
            //    orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            //}


            //// Xunit.Assert.Equal(orders.Count(), order_db.Count());

            //Logout();

        }

        [Fact]
        public void Transferee_Housingpage_ShouldCheckHousingsummary()
        {

            initialsteps();

            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#housing");

                delay(800);

                // File Details
                //var a = db_order.HomeFinding.HousingBudget.ToString().Replace(".","");
                //var b = GetElement(_driver, By.Id("HousingBudget"), 10);
                //var c = b.Replace("[^0-9]", "");
                Xunit.Assert.Equal(db_order.HomeFinding.HousingBudget.ToString().Replace(".", ""), GetElement(_driver, By.Id("HousingBudget"), 10).Replace("$", "").Replace(".", "").Replace(",", ""));
                Xunit.Assert.Equal(db_order.HomeFinding.NumberOfBedrooms.ToString(), _driver.FindElement(By.Id("NumberOfBedrooms")).Text);
                Xunit.Assert.Equal(db_order.Pets.Count().ToString(), _driver.FindElement(By.Id("PetsCount")).Text.Replace(".", ""));

                // Important Dates

                // Housing details

                _driver.FindElement(By.Id("backButton")).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }


            // Xunit.Assert.Equal(orders.Count(), order_db.Count());

            Logout();

        }

        [Fact]
        public void Transferee_Housingpage_ShouldCheckPropertiesCount()
        {

            initialsteps();

            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#housing");

                delay(800);
                IList<IWebElement> properties = _driver.FindElements(By.Id("Listproperties"));
                IList<IWebElement> properties1 = _driver.FindElements(By.ClassName(".row.sectionList"));
                IList<IWebElement> properties2 = _driver.FindElements(By.CssSelector("li#Listproperties"));

                //Xunit.Assert.Equal(db_order.HomeFinding.HousingBudget.ToString().Replace(".", ""), GetElement(_driver, By.Id("HousingBudget"), 10).Replace("$", "").Replace(".", "").Replace(",", ""));
                //Xunit.Assert.Equal(db_order.HomeFinding.NumberOfBedrooms.ToString(), _driver.FindElement(By.Id("NumberOfBedrooms")).Text);

                // works except for the last property vella koplin 5 shown but actual 8 prop

                Xunit.Assert.Equal(db_order.HomeFinding.HomeFindingProperties.Count().ToString(), properties.Count().ToString());

                // Important Dates

                // Housing details

                _driver.FindElement(By.Id("backButton")).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }


            // Xunit.Assert.Equal(orders.Count(), order_db.Count());

            Logout();

        }


        [Fact]
        public void Transferee_Housingpage_ShouldCheckFilter()
        {

            initialsteps();

            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#housing");
                delay(800);

                //var all_count = db_order.HomeFinding.HomeFindingProperties.Count();
                //var new_count = db_order.HomeFinding.HomeFindingProperties.Count();
                var liked_count = db_order.HomeFinding.HomeFindingProperties.Count(x => x.Liked == true);
                //var disliked_count = db_order.HomeFinding.HomeFindingProperties.Count();


                _driver.FindElement(By.Id("allfilter")).Click();




                _driver.FindElement(By.Id("newfilter")).Click();
                _driver.FindElement(By.Id("likedfilter")).Click();



                IList<IWebElement> liked_properties = _driver.FindElements(By.CssSelector("li#Listproperties"));
                _driver.FindElement(By.Id("dislikedfilter")).Click();
                var a = liked_properties.Count(x => x.Displayed).ToString();

                Xunit.Assert.Equal(liked_count.ToString(), liked_properties.Count(x => x.Displayed).ToString());

                // Important Dates

                // Housing details

                _driver.FindElement(By.Id("backButton")).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }


            // Xunit.Assert.Equal(orders.Count(), order_db.Count());

            Logout();

        }


        private string GetElement(IWebDriver driver, By by, int tries)
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
        private IWebElement GetElementClick(IWebDriver driver, By by, int tries)
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
