using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using System.Threading.Tasks;

namespace Odin.UITests
{
    class Globals
    {

        //public static String FILE_NAME = "Output.txt"; // Modifiable
        //public static readonly String CODE_PREFIX = "US-"; // Unmodifiable
        
        //Local Url 
        public static String Url_Localhost = "https://localhost:44357"; // Modifiable
        //public static String Url_Localhost = "https://localhost/" + new SslStream(null).SslProtocol;
        public static readonly String email_pm_valid = "Odinpm@dwellworks.com";
        public static readonly String pass_pm_valid = "OdinOdin5$";
        
    }
}
