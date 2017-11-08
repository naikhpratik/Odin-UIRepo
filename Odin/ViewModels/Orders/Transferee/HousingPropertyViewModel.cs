using System;
using Odin.Data.Core.Models;
using Odin.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Odin.ViewModels.Orders.Transferee
{
    public class HousingPropertyViewModel
    {
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

        public string PropertyStreet1 { get; set; }
        public string PropertyStreet2 { get; set; }
        public string PropertyCity { get; set; }
        public string PropertyState { get; set; }

        [Display(Name = "Availability Date:")]
        [DisplayFormat(NullDisplayText = "Unknown", DataFormatString = "{0:d}")]
        public DateTime? PropertyAvailabilityDate { get; set; }

        //public string Id { get; set; }

        //[Display(Name = "Location\nPhotos:")]
        //public string Photos { get; set; }

        //public string Street1 { get; set; }
        //public string Street2 { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //[Display(Name = "Address:")]
        //public string Address => Street1 + (Street2 ?? "") + (City) + (State);

        //public DateTime? AvailabilityDate { get; set; }
        //[Display(Name = "Available or Not\nDate:")]
        //public string Availability => AvailabilityDate >= DateTime.Now ? "Available " + DateHelper.GetViewFormat(AvailabilityDate) : "Not Available";

        //public NumberOfBathroomsType NumberOfBathrooms { get; set; }
        //public int? NumberOfBedrooms { get; set; }
        //public decimal Amount { get; set; }
        //public int? SquareFootage { get; set; }
        //[Display(Name = "BD | BA\nRent | Sq. Ft:")]
        //public string Specifications => NumberOfBedrooms + " | " + NumberOfBathrooms + Amount + " | " + SquareFootage;

        //public DateTime? ViewingDate { get; set; }
        //[Display(Name = "Schedule\na Viewing:")]
        //public string ViewingDateDisplay => DateHelper.GetViewFormat(ViewingDate);

        //[Display(Name = "Like or\nDislike:")]
        //public bool? IsLike { get; set; }

        //[Display(Name = "Property\nComments:")]
        //public IEnumerable<string> Comments { get; set; }
    }
}
