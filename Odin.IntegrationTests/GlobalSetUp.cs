using System.Configuration;
using System.Data.Entity.Migrations;
using NUnit.Framework;
using Odin.Data.Persistence;
using Odin.Helpers;
using Odin.IntegrationTests.Helpers;

namespace Odin.IntegrationTests
{
    [SetUpFixture]
    public class GlobalSetUp
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            MigrateDbToLatestVersion();

            Seed();

            if (ConfigurationManager.AppSettings["IsLocalTestingEnvironment"].Equals("true"))
            {
                if (!AzureStorageEmulatorManager.IsProcessRunning())
                    AzureStorageEmulatorManager.StartStorageEmulator();

                AzureStorageEmulatorManager.SetupImageContainer();
            }
        }

        private static void MigrateDbToLatestVersion()
        {
            var configuration = new Odin.Data.Persistence.Migrations.Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }

        public void Seed()
        {
            var context = new ApplicationDbContext();
            
            OrderHelper.ClearIntegrationOrders(context);

            context.SaveChanges();
        }
    }
}
