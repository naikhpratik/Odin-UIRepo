using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.UITests
{
    public class SeleniumTest
    {
        //private readonly int iisPort = ConfigHelper.GetWebServerPort();
        private readonly int iisPort = ConfigHelper.GetWebServerPort();
        private readonly string _applicationName;
        private Process _iisProcess;

        protected IWebDriver Driver { get; private set; }

        protected SeleniumTest(string applicationName)
        {
            _applicationName = applicationName;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            // Start IISExpress
            StartIIS();

            // Start Selenium drivers
            this.Driver = new PhantomJSDriver();
        }


        [TestCleanup]
        public void TestCleanup()
        {
            // Ensure IISExpress is stopped
            if (_iisProcess.HasExited == false)
            {
                _iisProcess.Kill();
            }

            // Stop all Selenium drivers
            this.Driver.Quit();
        }

        private void StartIIS()
        {
            var applicationPath = GetApplicationPath(_applicationName);
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            Debug.WriteLine(applicationPath);
            Debug.WriteLine(programFiles);
            _iisProcess = new Process
            {
                StartInfo = {
                    FileName = programFiles + @"\IIS Express\iisexpress.exe",
                    Arguments = $"/path:{applicationPath} /port:{iisPort}"
                }
            };
            _iisProcess.Start();
        }


        private static string GetApplicationPath(string applicationName)
        {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));
            return Path.Combine(solutionFolder, applicationName);
        }


        protected string GetAbsoluteUrl(string relativeUrl)
        {
            if (!relativeUrl.StartsWith("/"))
            {
                relativeUrl = "/" + relativeUrl;
            }
            return $"http://localhost:{iisPort}{relativeUrl}";
        }


    }
}
