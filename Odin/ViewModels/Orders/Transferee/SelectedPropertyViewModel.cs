using Ganss.XSS;
using Microsoft.AspNet.Identity;
using Odin.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using Odin.Data.Core.Models;

namespace Odin.ViewModels.Orders.Transferee
{
    public class SelectedPropertyViewModel
    {
        public String Id { get; set; }
        public bool Deleted { get; set; }
        public string propertyId { get; set; }
        public String OrderId { get; set; }
        public string pmEmail { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText ="", HtmlEncode = false)]
        [Display(Name = "Address")]
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
        
        [Display(Name = "Street 1")]
        public String PropertyStreet1 { get; set; }

        [Display(Name = "Street 2")]
        public String PropertyStreet2 { get; set; }

        [Display(Name = "City")]
        public String PropertyCity { get; set; }

        [Display(Name = "State")]
        public String PropertyState { get; set; }

        [Display(Name = "Postal Code")]
        public String PropertyPostalCode { get; set; }        

        [Display(Name = "Beds")]
        [DisplayFormat(NullDisplayText = "NA")]
        public int? PropertyNumberOfBedrooms { get; set; }

        [Display(Name = "Baths")]
        [DisplayFormat(NullDisplayText = "NA", DataFormatString = "{0:N1}", ApplyFormatInEditMode = true)]
        public decimal? PropertyNumberOfBathrooms { get; set; }

        [Display(Name = "Sq. Footage")]
        [DisplayFormat(NullDisplayText = "NA")]
        public int? PropertySquareFootage { get; set; }

        [Display(Name = "Rent")]
        [DisplayFormat(NullDisplayText = "NA", DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]        

    }
}
