﻿using Microsoft.WindowsAzure.Storage.Blob;
using Odin.Domain;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace Odin.Helpers
{
    public static class AzureStorageEmulatorManager
    {
        public static bool IsProcessRunning()
        {
            bool status;

            using (Process process = Process.Start(StorageEmulatorProcessFactory.Create(ProcessCommand.Status)))
            {
                if (process == null)
                {
                    throw new InvalidOperationException("Unable to start process.");
                }

                status = GetStatus(process);
                process.WaitForExit();
            }

            return status;
        }

        public static void StartStorageEmulator()
        {
            if (!IsProcessRunning())
            {
                ExecuteProcess(ProcessCommand.Start);
            }
        }

        public static void StopStorageEmulator()
        {
            if (IsProcessRunning())
            {
                ExecuteProcess(ProcessCommand.Stop);
            }
        }

        public static void SetupImageContainer()
        {
            if (ConfigurationManager.AppSettings["IsLocalTestingEnvironment"].Equals("true"))
            {
                CloudBlobContainer container = new ImageStore().GetImageContainer();
                container.CreateIfNotExists();
                // set the proper permissions so the images can be fetched
                BlobContainerPermissions containerPermissions = container.GetPermissions();
                containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                container.SetPermissions(containerPermissions);
            }
        }

        private static void ExecuteProcess(ProcessCommand command)
        {
            string error;

            using (Process process = Process.Start(StorageEmulatorProcessFactory.Create(command)))
            {
                if (process == null)
                {
                    throw new InvalidOperationException("Unable to start process.");
                }

                error = GetError(process);
                process.WaitForExit();
            }

            if (!String.IsNullOrEmpty(error))
            {
                throw new InvalidOperationException(error);
            }
        }

        private static class StorageEmulatorProcessFactory
        {
            public static ProcessStartInfo Create(ProcessCommand command)
            {
                return new ProcessStartInfo
                {
                    FileName = ConfigurationManager.AppSettings["WindowsAzureStorageEmulatorPath"],
                    Arguments = command.ToString().ToLower(),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
            }
        }

        private enum ProcessCommand
        {
            Start,
            Stop,
            Status
        }

        private static bool GetStatus(Process process)
        {
            string output = process.StandardOutput.ReadToEnd();
            string isRunningLine = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).SingleOrDefault(line => line.StartsWith("IsRunning"));

            if (isRunningLine == null)
            {
                return false;
            }

            return Boolean.Parse(isRunningLine.Split(':').Select(part => part.Trim()).Last());
        }

        private static string GetError(Process process)
        {
            string output = process.StandardError.ReadToEnd();
            return output.Split(':').Select(part => part.Trim()).Last();
        }

    }
}
