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
        public HousingViewModel()
        {
            Properties = new List<HousingPropertyViewModel>();
        }

        public string Id { get; set; }

        [Display(Name = "Budget:")]
        [DisplayFormat(DataFormatString = "{0:c}", NullDisplayText = "NA")]
        public decimal? HousingBudget { get; set; }

        [Display(Name = "Bedrooms:")]
        [DisplayFormat(NullDisplayText = "NA")]
        public int? NumberOfBedrooms { get; set; }

        [Display(Name = "Bathrooms:")]
        public String NumberOfBathroomsString { get { return this.NumberOfBathrooms != null ? this.NumberOfBathrooms.Name : "NA"; } }
        public NumberOfBathroomsType NumberOfBathrooms { private get; set; }

        [Display(Name = "Housing Type:")]
        public String HousingTypeString { get { return this.HousingType != null ? this.HousingType.Name : "NA"; } }
        public HousingType HousingType { private get; set; }

        [Display(Name = "Pets:")]
        [DisplayFormat(NullDisplayText = "No Pets")]
        public int NumberOfPets { get; set; }

        [Display(Name = "Spouse/Kids:")]
        [DisplayFormat(NullDisplayText = "No Family")]
        public string SpouceAndKids { get; set; }

        [Display(Name = "Distance From Work:")]
        [DisplayFormat(NullDisplayText = "NA")]
        public int? MaxCommute { get; set; }

        [Display(Name = "Transportation:")]
        public String TransportationTypeString { get { return this.TransportationType != null ? this.TransportationType.Name : "NA"; } }
        public TransportationType TransportationType { private get; set; }

        public IEnumerable<HousingPropertyViewModel> Properties { get; set; }
    }
}