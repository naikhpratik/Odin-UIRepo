using System;
using Odin.Data.Core.Models;
using Odin.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Odin.ViewModels.Orders.Transferee
{
    public class HousingPropertyViewModel
    {
        public String OrderId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText ="", HtmlEncode = false)]
        public string PropertyAddress { get
            {
                var addressMarkup = PropertyStreet1 + "<br />";
                if (PropertyStreet2 != null && PropertyStreet2.Length > 0)
                {
                    addressMarkup += PropertyStreet2 + "<br />";
                }
                addressMarkup += PropertyCity + ", " + PropertyState;

                return addressMarkup;
            }
        }
        
        [Display(Name = "Street 1:")]
        [Required(ErrorMessage = "Street is required")]
        public string PropertyStreet1 { get; set; }

        [Display(Name = "Street 2:")]
        public string PropertyStreet2 { get; set; }

        [Display(Name = "City:")]
        [Required(ErrorMessage = "City is required")]
        public string PropertyCity { get; set; }

        [Display(Name = "State Abbr:")]
        [Required(ErrorMessage = "State Abbr. is required")]
        public string PropertyState { get; set; }

        [Display(Name = "Availability Date:")]
        [DisplayFormat(NullDisplayText = "Unknown", DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime? PropertyAvailabilityDate { get; set; }

        [Display(Name = "BD")]
        [DisplayFormat(NullDisplayText = "NA")]
        public int PropertyNumberOfBedrooms { get; set; }

        [Display(Name = "BA")]
        [DisplayFormat(NullDisplayText = "NA", ConvertEmptyStringToNull = true)]
        public String PropertyNumberOfBathroomsName { get; set; }

        [Display(Name = "Sq. Ft.")]
        [DisplayFormat(NullDisplayText = "NA", DataFormatString = "{0:N0}")]
        public int PropertySquareFootage { get; set; }

        [Display(Name = "Rent")]
        [DisplayFormat(NullDisplayText = "NA", DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public Decimal PropertyAmount { get; set; }
    }
}
