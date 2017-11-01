using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Odin.ViewModels.Shared;
using System.ComponentModel.DataAnnotations;
using Odin.Data.Core.Models;

namespace Odin.ViewModels.Orders.Transferee
{
    public class HousingViewModel
    {
        [Display(Name = "Budget:")]

        [DisplayFormat(DataFormatString = "{0:c}", NullDisplayText = "NA")]
        public decimal? HousingBudget { get; set; }

        [Display(Name = "Bedrooms:")]
        [DisplayFormat(NullDisplayText = "NA")]
        public int? NumberOfBedrooms { get; set; }

        [Display(Name = "Bathrooms:")]
        [DisplayFormat(NullDisplayText = "NA")]
        public NumberOfBathroomsType NumberOfBathrooms { get; set; }

        [Display(Name = "Housing Type:")]
        [DisplayFormat(NullDisplayText = "NA")]
        public HousingType HousingType { get; set; }
    }
}