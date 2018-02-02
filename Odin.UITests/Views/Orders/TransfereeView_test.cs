﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.Helpers;
using Odin.UITests.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;
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
    //[Collection("Our Test Collection #1")]
    [TestClass]
    public class TransfereeView_test
    {
        private string baseURL = Globals.Url_Localhost;
        private IWebDriver _driver;
        private ApplicationDbContext _context;
        private UnitOfWork _unitOfWork;
        private UsersRepository userRepo;
        public IEnumerable<Order> orderFrom_db;
        private IList<IWebElement> orders;
        //private object method_Name;
        private HelperMethod help;

        public TransfereeView_test()
        {
            //DesiredCapabilities capabilities = DesiredCapabilities.Chrome();
            //ChromeOptions options = new ChromeOptions();
            //options.AddArguments("--incognito");
            //options.ToCapabilities();
            //_driver = new ChromeDriver();

            _driver = new PhantomJSDriver();
            help = new HelperMethod(_driver);
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
            userRepo = new UsersRepository(_context);

        }

        [TestMethod]
        public void Transferee_IntakePage_ShouldCheckForTransfereeDetails()
        {

            help.initialsteps();
            orders = help.getOrders();

            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                orders.ElementAt(i).Click();


                help.delay(800);
                //Check for contact Info

                Xunit.Assert.Equal(db_order.Transferee.FullName, help.GetElement(_driver, By.Id("Transferee_FullName"), 10));
                Xunit.Assert.Equal(db_order.Transferee.Email, help.GetElement(_driver, By.Id("Transferee_Email"), 10));

                _driver.Navigate().GoToUrl(this.baseURL + "/Orders");
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }


            // Xunit.Assert.Equal(orders.Count(), order_db.Count());

            help.Logout();

        }

        [TestMethod]
        public void Transferee_Intakepage_ShouldCheckandUpdateDestinationLocation()
        {

            help.initialsteps();
            orders = help.getOrders();
            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id); ;
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id);
                help.delay(800);
                //Check for Destiantion and Departure Info

                /**
                //var city = _driver.FindElement(By.CssSelector("span#spanDestinationCity.intake-span")).Text;
                //var state = _driver.FindElement(By.Id("spanDestinationState")).Text;
                //var country = _driver.FindElement(By.Id("spanDestinationCountry")).Text;
                // Xunit.Assert.Equal(db_order.DestinationCity, _driver.FindElement(By.CssSelector("span#spanDestinationCity.intake-span")).Text);
                // Xunit.Assert.Equal(db_order.DestinationState, _driver.FindElement(By.CssSelector("span#spanDestinationState.intake-span")).Text);
                // Xunit.Assert.Equal(db_order.DestinationCountry, _driver.FindElement(By.CssSelector("span#spanDestinationCountry.intake-span")).Text);
                **/
                var click_expand = help.GetElementClick(_driver, By.XPath("(//img[@class = 'intake-expand-img'])[6]"), 10);
                var click_collapse = help.GetElementClick(_driver, By.XPath("(//img[@class = 'intake-collapse-img'])[6]"), 10);
                ((IJavaScriptExecutor)_driver).ExecuteScript("scroll(0,400)");
                click_expand.Click();
                click_collapse.Click();
                //delay(100);
                click_expand.Click();


                //** Destination Location **//
                Xunit.Assert.Equal(db_order.DestinationCity, help.GetElement(_driver, By.Id("spanDestinationCity"), 10));
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

            help.Logout();

        }

        [TestMethod]
        public void Transferee_Intakepage_ShouldCheckandUpdateDepartureLocation()
        {

            help.initialsteps();
            orders = help.getOrders();
            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id);
                help.delay(800);
                //Check for Destination and Departure Info

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

                var click_expand = help.GetElementClick(_driver, By.XPath("(//img[@class = 'intake-expand-img'])[7]"), 10);
                var click_collapse = help.GetElementClick(_driver, By.XPath("(//img[@class = 'intake-collapse-img'])[7]"), 10);
                ((IJavaScriptExecutor)_driver).ExecuteScript("scroll(0,400)");
                click_expand.Click();
                click_collapse.Click();
                //delay(100);
                click_expand.Click();


                Xunit.Assert.Equal(db_order.OriginCity, help.GetElement(_driver, By.Id("spanOriginCity"), 10));
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

            help.Logout();

        }

        [TestMethod]
        public void Transferee_Intakepage_ShouldCheckandUpdateRelocationDates()
        {

            help.initialsteps();

            orders = help.getOrders();
            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
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

                help.delay(800);
                Xunit.Assert.Equal(DateHelper.GetViewFormat(db_order.PreTripDate), help.GetElement(_driver, By.Id("spanPreTripDate"), 10));
                Xunit.Assert.Equal(DateHelper.GetViewFormat(db_order.EstimatedArrivalDate), _driver.FindElement(By.Id("spanEstimatedArrivalDate")).Text);
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

            help.Logout();

        }

        [TestMethod]
        public void Transferee_Intakepage_ShouldCheckandUpdateGeneral()
        {

            help.initialsteps();
            orders = help.getOrders();
            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id);
                help.delay(800);

                var click_expand = help.GetElementClick(_driver, By.XPath("(//img[@class = 'intake-expand-img'])[12]"), 10);
                var click_collapse = help.GetElementClick(_driver, By.XPath("(//img[@class = 'intake-collapse-img'])[12]"), 10);
                ((IJavaScriptExecutor)_driver).ExecuteScript("scroll(0,1200)");
                click_expand.Click();
                click_collapse.Click();
                //delay(1000);
                click_expand.Click();

                help.delay(100);
                //Xunit.Assert.Equal(DateHelper.GetViewFormat(db_order.IsRush), GetElement(_driver, By.Id("spanRush"), 10));

                if (db_order.IsRush)
                {
                    Xunit.Assert.Equal("Yes", help.GetElement(_driver, By.Id("spanRush"), 10));
                }
                else
                    Xunit.Assert.Equal("No", help.GetElement(_driver, By.Id("spanRush"), 10));

                if (db_order.IsVip)
                {
                    Xunit.Assert.Equal("Yes", help.GetElement(_driver, By.Id("spanVip"), 10));
                }
                else
                    Xunit.Assert.Equal("No", help.GetElement(_driver, By.Id("spanVip"), 10));

                if (db_order.IsAssignment)
                {
                    Xunit.Assert.Equal("Temporary", help.GetElement(_driver, By.Id("spanAssignment"), 10));
                }
                else
                    Xunit.Assert.Equal("Permanent", help.GetElement(_driver, By.Id("spanAssignment"), 10));

                if (db_order.IsInternational)
                {
                    Xunit.Assert.Equal("International", help.GetElement(_driver, By.Id("spanInternational"), 10));
                }
                else
                    Xunit.Assert.Equal("Domestic", help.GetElement(_driver, By.Id("spanInternational"), 10));


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

            help.Logout();

        }

        //check after merge
        [TestMethod]
        public void Transferee_Intakepage_ShouldCheckServicesonDetailspage()
        {

            help.initialsteps();
            orders = help.getOrders();
            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id);
                help.delay(800);

                var totalservices_count = db_order.Services.Count();
                var selectedservices_count = db_order.Services.Count(x => x.Selected == true);

                IList<IWebElement> intake_services = _driver.FindElements(By.XPath("//div[@data-entity-collection = 'services']"));

                //Xunit.Assert.Equal(intake_services.Count(), db_order.Services.Count());


                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#details");
                help.delay(800);
                IList<IWebElement> details_services = _driver.FindElements(By.XPath("//ul[@data-entity-collection = 'services']"));

                Xunit.Assert.Equal(selectedservices_count, details_services.Count());

                _driver.FindElement(By.Id("backButton")).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }

            help.Logout();

        }

        [TestMethod]
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

            //help.Logout();

        }

        [TestMethod]
        public void Transferee_Housingpage_ShouldCheckHousingsummary()
        {

            help.initialsteps();
            orders = help.getOrders();
            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#housing");

                help.delay(800);
                if (db_order.HomeFinding != null && !_driver.FindElement(By.Id("housingsectionTitle")).Text.Equals("Selected Home"))
                {
                    // File Details
                    var Db_housingbuget = db_order.HomeFinding.HousingBudget.ToString().Replace(".", "");
                    var Ui_housingbudget = help.GetElement(_driver, By.Id("HousingBudget"), 10).Replace("$", "").Replace(".", "").Replace(",", "");
                    var Db_Noofbedrooms = db_order.HomeFinding.NumberOfBedrooms.ToString();
                    var Ui_Noofbedrooms = _driver.FindElement(By.Id("NumberOfBedrooms")).Text;
                    var Db_Pets = db_order.Pets.Count().ToString();
                    var Ui_Pets = _driver.FindElement(By.Id("PetsCount")).Text.Replace(".", "");

                    if (Db_housingbuget == "")
                        Xunit.Assert.Equal("No Preference", Ui_housingbudget);
                    else
                        Xunit.Assert.Equal(Db_housingbuget, Ui_housingbudget);

                    if (Db_Noofbedrooms == "")
                        Xunit.Assert.Equal("No Preference", Ui_Noofbedrooms);
                    else
                        Xunit.Assert.Equal(Db_Noofbedrooms, Ui_Noofbedrooms);

                    if (Db_Pets == "0")
                        Xunit.Assert.Equal("No Pets", Ui_Pets);
                    else
                        Xunit.Assert.Equal(Db_Pets, Ui_Pets);

                    //Xunit.Assert.Equal(db_order.HomeFinding.HousingBudget.ToString().Replace(".", ""), help.GetElement(_driver, By.Id("HousingBudget"), 10).Replace("$", "").Replace(".", "").Replace(",", ""));
                    //Xunit.Assert.Equal(db_order.HomeFinding.NumberOfBedrooms.ToString(), _driver.FindElement(By.Id("NumberOfBedrooms")).Text);
                    //Xunit.Assert.Equal(db_order.Pets.Count().ToString(), _driver.FindElement(By.Id("PetsCount")).Text.Replace(".", ""));

                    // Important Dates

                    // Housing details
                }
                _driver.FindElement(By.Id("backButton")).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }


            // Xunit.Assert.Equal(orders.Count(), order_db.Count());

            help.Logout();

        }

        [TestMethod]
        public void Transferee_Housingpage_ShouldCheckPropertiesCount()
        {

            help.initialsteps();
            orders = help.getOrders();
            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#housing");
                //check the title and  then because of change in pages
                help.delay(800);
                if (db_order.HomeFinding != null && !help.GetElement(_driver, By.Id("housingsectionTitle"), 10).Equals("Selected Home"))
                {
                    IList<IWebElement> properties = _driver.FindElements(By.Id("Listproperties"));
                    IList<IWebElement> properties1 = _driver.FindElements(By.ClassName(".row.sectionList"));
                    IList<IWebElement> properties2 = _driver.FindElements(By.CssSelector("li#Listproperties"));

                    //Xunit.Assert.Equal(db_order.HomeFinding.HousingBudget.ToString().Replace(".", ""), GetElement(_driver, By.Id("HousingBudget"), 10).Replace("$", "").Replace(".", "").Replace(",", ""));
                    //Xunit.Assert.Equal(db_order.HomeFinding.NumberOfBedrooms.ToString(), _driver.FindElement(By.Id("NumberOfBedrooms")).Text);

                    // works except for the last property vella koplin 5 shown but actual 8 prop

                    //if (!_driver.FindElement(By.Id("housingsectionTitle")).Text.Equals("Selected Home"))
                    Xunit.Assert.Equal(db_order.HomeFinding.HomeFindingProperties.Count(x => x.Deleted != true).ToString(), properties.Count().ToString());

                }
                _driver.FindElement(By.Id("backButton")).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));
            }

            help.Logout();

        }


        [TestMethod]
        public void Transferee_Housingpage_ShouldCheckFilter()
        {

            help.initialsteps();
            orders = help.getOrders();
            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#housing");
                help.delay(1800);

                //var all_count = db_order.HomeFinding.HomeFindingProperties.Count();
                //var new_count = db_order.HomeFinding.HomeFindingProperties.Count();
                var liked_count = db_order.HomeFinding != null ? db_order.HomeFinding.HomeFindingProperties.Count(x => x.Liked == true) : 0;
                //var disliked_count = db_order.HomeFinding.HomeFindingProperties.Count();

                if (db_order.HomeFinding != null && !help.GetElement(_driver, By.Id("housingsectionTitle"), 10).Equals("Selected Home"))
                {
                    //help.GetElementClick(_driver, By.Id("allfilter"), 10).Click();
                    _driver.FindElement(By.Id("allfilter")).Click();
                    _driver.FindElement(By.Id("newfilter")).Click();
                    _driver.FindElement(By.Id("likedfilter")).Click();

                    IList<IWebElement> liked_properties = _driver.FindElements(By.CssSelector("li#Listproperties"));

                    var a = liked_properties.Count(x => x.Displayed).ToString();
                    var res1 = liked_count.ToString();
                    var res2 = liked_properties.Count(x => x.Displayed).ToString();
                    Xunit.Assert.Equal(liked_count.ToString(), liked_properties.Count(x => x.Displayed).ToString());

                    _driver.FindElement(By.Id("dislikedfilter")).Click();



                    // Important Dates

                    // Housing details
                }
                _driver.FindElement(By.Id("backButton")).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));

            }

            // Xunit.Assert.Equal(orders.Count(), order_db.Count());
            help.Logout();

        }

        [TestMethod]
        public void Transferee_Housingpage_ShouldAddProperty()
        {

            help.initialsteps();
            orders = help.getOrders();
            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#housing");
                help.delay(800);

                var before_properties = db_order.HomeFinding.HomeFindingProperties.Count(x => x.Deleted == false);


                if (db_order.HomeFinding != null && !help.GetElement(_driver, By.Id("housingsectionTitle"), 10).Equals("Selected Home"))
                {
                    help.GetElementClick(_driver, By.Id("addProperty"), 10).Click();
                    _driver.FindElement(By.Id("PropertyStreet1")).SendKeys("11145 Meril Rd");
                    _driver.FindElement(By.Id("PropertyCity")).SendKeys("Singheshwar");
                    _driver.FindElement(By.XPath("(//option[@value = 'CA'])")).Click();
                    _driver.FindElement(By.Id("PropertyPostalCode")).SendKeys("44567");
                    _driver.FindElement(By.Id("savePropertyAdd")).Click();

                    _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#housing");
                    help.delay(800);
                    IList<IWebElement> after_properties = _driver.FindElements(By.Id("Listproperties"));
                    //help.delay(200);
                    Xunit.Assert.Equal(before_properties, after_properties.Count() - 1);
                }
                ///help.delay(2000);
                help.GetElementClick(_driver, By.Id("backButton"), 10).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));
            }


            // Xunit.Assert.Equal(orders.Count(), order_db.Count());
            help.Logout();

        }


        [TestMethod]
        public void Transferee_Housingpage_ShouldRemoveProperty()
        {

            help.initialsteps();
            orders = help.getOrders();
            for (int i = 0; i < orders.Count(); i++)
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#housing");
                help.delay(800);

                var before_properties = db_order.HomeFinding.HomeFindingProperties.Count(x => x.Deleted == false);

                IList<IWebElement> list_properties = _driver.FindElements(By.Id("Listproperties"));


                if (db_order.HomeFinding != null && list_properties.Count > 0 && !help.GetElement(_driver, By.Id("housingsectionTitle"), 10).Equals("Selected Home"))
                {
                    list_properties.First().Click();
                    //help.delay(800);
                    help.GetElementClick(_driver, By.Id("removeProperty"), 10).Click();

                    if (_driver.GetType() == typeof(PhantomJSDriver))
                    {
                        PhantomJSDriver phantom = (PhantomJSDriver)_driver;
                        phantom.ExecuteScript("window.alert = function(){}");
                        phantom.ExecuteScript("window.confirm = function(){return true;}");
                    }
                    else
                        _driver.SwitchTo().Alert().Accept();

                    IList<IWebElement> after_properties = _driver.FindElements(By.Id("Listproperties"));
                    Xunit.Assert.Equal(before_properties, after_properties.Count());
                }

                help.GetElementClick(_driver, By.Id("backButton"), 10).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));
            }
            //Xunit.Assert.Equal(orders.Count(), order_db.Count());
            help.Logout();

        }

        [TestMethod]
        public void Transferee_Housingpage_ShouldSelectProperties()
        {

            help.initialsteps();
            orders = help.getOrders();
            for (int i = 0; i < orders.Count(); i++)
            {

                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#housing");
                //check the title and  then because of change in pages
                help.delay(800);
                if (db_order.HomeFinding != null && !help.GetElement(_driver, By.Id("housingsectionTitle"), 10).Equals("Selected Home"))
                {
                    IList<IWebElement> properties = _driver.FindElements(By.Id("Listproperties"));

                    properties.First().Click();

                    help.GetElementClick(_driver, By.Id("selectProperty"), 10).Click();
                    help.delay(800);
                    var text = help.GetElement(_driver, By.Id("housingsectionTitle"), 10);
                    Xunit.Assert.True(help.GetElement(_driver, By.Id("housingsectionTitle"), 10).Equals("Selected Home"));
                }
                help.GetElementClick(_driver, By.Id("backButton"), 10).Click();
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));
            }

            help.Logout();

        }

        [TestMethod]
        public void Transferee_Housingpage_ShouldDeselectProperties()
        {

            help.initialsteps();
            orders = help.getOrders();
            for (int i = 0; i < orders.Count(); i++)
            {
                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#housing");
                //check the title and  then because of change in pages
                help.delay(800);
                if (db_order.HomeFinding != null && help.GetElement(_driver, By.Id("housingsectionTitle"), 10).Equals("Selected Home"))
                {
                    help.GetElementClick(_driver, By.Id("deselectProperty"), 10).Click();
                    help.delay(800);
                    Xunit.Assert.True(help.GetElement(_driver, By.Id("housingsectionTitle"), 10).Equals("Housing Summary"));
                }
                help.GetElementClick(_driver, By.Id("backButton"), 10);
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));
            }

            help.Logout();

        }

        [TestMethod]
        public void Transferee_Housingpage_ShouldEditPropertiesinModal()
        {
            help.initialsteps();
            orders = help.getOrders();
            for (int i = 0; i < orders.Count(); i++)
            {
                var order_id = orders.ElementAt(i).GetAttribute("data-order-id");
                var db_order = _unitOfWork.Orders.GetOrderById(order_id);
                _driver.Navigate().GoToUrl(this.baseURL + "/Orders/Transferee/" + order_id + "#housing");
                //check the title and  then because of change in pages
                help.delay(800);
                if (db_order.HomeFinding != null && !help.GetElement(_driver, By.Id("housingsectionTitle"), 10).Equals("Selected Home"))
                {
                    IList<IWebElement> properties = _driver.FindElements(By.Id("Listproperties"));
                    properties.ElementAt(help.GetRandomNo(properties)).Click();

                    help.delay(800);
                    _driver.FindElement(By.Id("editProperty")).Click();

                    _driver.FindElement(By.XPath("(//input[@class = 'form-control intake-input'])[1]")).SendKeys("2");
                    _driver.FindElement(By.XPath("(//input[@class = 'form-control intake-input'])[2]")).SendKeys("2");
                    _driver.FindElement(By.XPath("(//input[@class = 'form-control intake-input'])[3]")).SendKeys("1.5");
                    _driver.FindElement(By.XPath("(//input[@class = 'form-control intake-input'])[4]")).SendKeys("232.453");

                    _driver.FindElement(By.Id("editProperty")).Click();
                    
                    //No assert statements because the modal completing its task and gettign closed 
                    Xunit.Assert.True(help.closeModal(_driver));
                }
                help.GetElementClick(_driver, By.Id("backButton"), 10);
                orders = _driver.FindElements(By.Id("rowclickableorderRow"));
            }

            help.Logout();

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


        [TestCleanup]
        public void Dispose()
        {
            //System.Diagnostics.Debug.WriteLine("Calling Method : " + method_Name);
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
