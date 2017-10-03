using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Dtos
{
    public class OrderIndexDto
    {
        public List<TransfereeIndexDto> Transferees;
    }

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

        public List<ServicesDto> Services;
    }

    public class ServicesDto
    {
        public string Name;
        public DateTime Completed;
    }
}

/*
{

"Transferees": [
{
    "FirstName": "String",
    "Middle": "String",
    "LastName": "String",

    "Rmc": "String",
    "Company": "String",

    "NumberOfSecheduledServices": "Int",
    "NumberOfServices": "Int",
    "NumberOfCompletedServices": "Int",

    "LastContacted": "Date",
    "Manager": "String",
    "ManagerPhone": "String",
    "PreTrip": "Date",
    "FinalArrival": "Date",

    "NumberOfAlerts": "Int",

    "Services": [
    {
        "Name": "String",
        "Completed": "Date"
    }
    ]
}
]    
}

    */