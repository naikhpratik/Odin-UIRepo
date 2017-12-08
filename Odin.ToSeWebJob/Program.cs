using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Ninject;

namespace Odin.ToSeWebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            using (IKernel kernel = new StandardKernel(new MyBindings()))
            {
                var config = new JobHostConfiguration
                {
                    JobActivator = new MyJobActivator(kernel)
                };

                if (config.IsDevelopment)
                {
                    config.UseDevelopmentSettings();
                }
                
                config.UseCore();

                var host = new JobHost(config);
                // The following code ensures that the WebJob will be running continuously
                host.RunAndBlock();
            }

        }
    }
}
