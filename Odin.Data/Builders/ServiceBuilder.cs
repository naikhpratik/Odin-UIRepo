using Bogus;
using System.Collections.Generic;
using System.Linq;
using Odin.Data.Core.Models;
using System;

namespace Odin.Data.Builders
{
    public class ServiceBuilder{       

        public static Service New(ServiceType serviceType)
        {
            return new Service()
            {
                ServiceType = serviceType,
                ServiceTypeId = serviceType.Id,
                ScheduledDate = DateTime.Now,
                CompletedDate = DateTime.Now
            };
        }
    }
}
