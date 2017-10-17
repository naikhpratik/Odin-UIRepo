using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Odin.Data.Core.Dtos
{
    public class TransfereeIndexDto
    {
        public TransfereeIndexDto()
        {
            Services = new Collection<ServiceDto>();
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

        public ICollection<ServiceDto> Services;
    }
}