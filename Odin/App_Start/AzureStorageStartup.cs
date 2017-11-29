using System.Configuration;
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

                AzureStorageEmulatorManager.SetupImageContainer();
            }
        }
    }
}