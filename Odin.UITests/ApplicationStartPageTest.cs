﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium.PhantomJS;
using System.Diagnostics;

namespace Odin.UITests
{
    [TestClass]
    public class ApplicationStartPageTest : IDisposable
    {
        private string baseURL = Globals.Url_Localhost;
        private readonly IWebDriver _driver;
        private string method_Name;
        public ApplicationStartPageTest()
        {
            _driver = new ChromeDriver();
            //_driver = new PhantomJSDriver();
        }

        private void initialsteps()
        {

            _driver.Navigate().GoToUrl(this.baseURL);
            _driver.Manage().Window.Maximize();
            
        }

        private void Delay(int msec)
        {
            Thread.Sleep(msec);
        }

        [Fact]
        public void ShouldLoadApplicationPage_test()
        {

            initialsteps();
            Xunit.Assert.Equal("Log in", _driver.Title);
            //method_Name = "ShouldLoadApplicationPage_test";
        }

        [Fact]
        public void ChromeLoginLogout_test()
        {


            //_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            //delay(500);
            initialsteps();
            Xunit.Assert.True(Login(Globals.email_pm_valid, Globals.pass_pm_valid));
            Xunit.Assert.True(Logout());

            //method_Name = "ChromeLoginLogout_test";
        }

        public Boolean Login(string username, string password)
        {
            if (_driver.Url.Split('/').Last().Contains("Login"))
            {
                _driver.FindElement(By.Id("Email")).SendKeys(username);
                _driver.FindElement(By.Id("Password")).SendKeys(password);
                _driver.FindElement(By.ClassName("btn-default")).Click();
                if (_driver.Url.Split('/').Last().Contains("Orders"))
                    Xunit.Assert.Equal("Orders Page", _driver.Title);
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Logout()
        {
            _driver.FindElement(By.Id("logoutForm")).Submit();
            if (_driver.Url.Split('/').Last().Contains("Login"))
            {
                _driver.Close();
                return true;
            }
            else
            { return false; }
        }

        // Can add additional functionality to verify email 
        [Fact]
        public void Forgotpasswordclick_test()
        {
            initialsteps();
            _driver.FindElement(By.Id("forgot_password")).Click();
            var word = "";
            if (_driver.Url.Split('/').Last().Equals("ForgotPassword"))
            {
                word = "Success";
            }
            else
            {
                word = "Failure";
            }
            Xunit.Assert.Equal("Success", word);
            method_Name = "Forgotpasswordclick_test";
        }

        [Fact]
        public void LoginPage_InvalidCredentails_ShouldCheckForErrors()
        {
            initialsteps();
            Xunit.Assert.True(Login("", ""));
            Xunit.Assert.Equal("The Email field is required.", _driver.FindElement(By.Id("validation_email")).Text);
            Xunit.Assert.Equal("The Password field is required.", _driver.FindElement(By.Id("validation_password")).Text);
            method_Name = "LoginPage_InvalidCredentails_ShouldCheckForErrors";
        }

        [TestCleanup]
        public void Dispose()
        {
            System.Diagnostics.Debug.WriteLine("Calling Method : " + method_Name);

            _driver.Quit();
            _driver.Dispose();
        }
    }
}
