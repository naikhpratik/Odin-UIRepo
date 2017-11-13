using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Odin.Helpers;

namespace Odin.App_Start
{
    public class AzureStorageStartup
    {
        public static void StartLocalAzureStorageEmulator()
        {
            if (ConfigurationManager.AppSettings["IsLocalTestingEnvironment"].Equals("true"))
            {
                if (!AzureStorageEmulatorManager.IsProcessRunning())
                {
                    AzureStorageEmulatorManager.StartStorageEmulator();
                }
            }
        }
    }
}