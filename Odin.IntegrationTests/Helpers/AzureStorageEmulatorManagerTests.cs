using System.Configuration;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Helpers;

namespace Odin.IntegrationTests.Helpers
{
    [TestClass]
    public class AzureStorageEmulatorManagerTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            StopStorageEmulatorIfRunning();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            StopStorageEmulatorIfRunning();
        }

        private void StopStorageEmulatorIfRunning()
        {
            if (ConfigurationManager.AppSettings["IsLocalTestingEnvironment"].Equals("false"))
                return;
            if (AzureStorageEmulatorManager.IsProcessRunning())
            {
                AzureStorageEmulatorManager.StopStorageEmulator();
            }
        }

        [TestMethod]
        public void StartStorageEmulator_WhenRunning_ShouldStartEmulator()
        {

            if (ConfigurationManager.AppSettings["IsLocalTestingEnvironment"].Equals("false"))
                return;
            if (AzureStorageEmulatorManager.IsProcessRunning())
            {
                AzureStorageEmulatorManager.StopStorageEmulator();
                AzureStorageEmulatorManager.IsProcessRunning().Should().BeFalse();
            }

            AzureStorageEmulatorManager.StartStorageEmulator();
            AzureStorageEmulatorManager.IsProcessRunning().Should().BeTrue();
        }

        [TestMethod]
        public void StopStorageEmulator_WhenRunning_EmulatorShouldBeStopped()
        {

            if (ConfigurationManager.AppSettings["IsLocalTestingEnvironment"].Equals("false"))
                return;
            if (!AzureStorageEmulatorManager.IsProcessRunning())
            {
                AzureStorageEmulatorManager.StartStorageEmulator();
                AzureStorageEmulatorManager.IsProcessRunning().Should().BeTrue();
            }

            AzureStorageEmulatorManager.StopStorageEmulator();
            AzureStorageEmulatorManager.IsProcessRunning().Should().BeFalse();
        }

    }
}
