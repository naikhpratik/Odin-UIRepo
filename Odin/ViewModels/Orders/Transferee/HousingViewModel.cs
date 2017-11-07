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
        [DisplayFormat(NullDisplayText = "NA")]
        public String NumberOfBathroomsName { get; set; }

        [Display(Name = "Housing Type:")]
        [DisplayFormat(NullDisplayText = "No Preference")]
        public String HousingTypeName { get; set; }

        private int? _PetsCount;
        [Display(Name = "Pets:")]
        [DisplayFormat(NullDisplayText = "No Pets")]
        public int? PetsCount
        {
            get { return _PetsCount; }
            set
            {
                if (value > 0)
                {
                    _PetsCount = value;
                }
            }
        }

        public int ChildrenCount { get; set; }
        public String SpouseName { get; set; }

        [Display(Name = "Spouse and Kids:")]
        [DisplayFormat(NullDisplayText = "No Family")]
        public string SpouseAndKids
        {
            get
            {
                bool hasSpouse = SpouseName != null && SpouseName.Length > 0;
                bool hasChildren = ChildrenCount > 0;

                if (hasSpouse || hasChildren)
                {
                   return (hasSpouse ? "Yes":"No") + " / " + ChildrenCount.ToString();
                }
                   
                return null;
            }
        }

        [Display(Name = "Distance From Work (min):")]
        [DisplayFormat(NullDisplayText = "NA")]
        public int? MaxCommute { get; set; }

        [Display(Name = "Transportation:")]
        [DisplayFormat(NullDisplayText = "NA")]
        public String TransportationTypeName { get; set; }

        public IEnumerable<HousingPropertyViewModel> Properties { get; set; }
    }
}