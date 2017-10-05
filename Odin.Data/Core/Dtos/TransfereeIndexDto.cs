using System;
using System.Collections.Generic;

namespace Odin.Data.Core.Dtos
{
    public class TransfereeIndexDto
    {
        public TransfereeIndexDto()
        {
            Services = new List<ServicesDto>();
        }
        public string FirstName;
        public string Middle;
        public string LastName;

        public string Rmc;
        public string Company;

        public int NumberOfScheduledServices;
        public int NumberOfServices;
        public int NumberOfCompletedServices;

        public DateTime? LastContacted;
        public string Manager;
        public string ManagerPhone;
        public DateTime? PreTrip;
        public DateTime? FinalArrival;

        public int NumberOfAlerts;

        public IEnumerable<ServicesDto> Services;
    }
}