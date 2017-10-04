using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Dtos
{
    public class OrderIndexDto
    {
        public IEnumerable<TransfereeIndexDto> Transferees;
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