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
    public class LeaseViewModel
    {
        public LeaseViewModel()
        {
            
        }

        public bool Deleted { get; set; }
        public string propertyId { get; set; }
        public String OrderId { get; set; }
        public String Id { get; set; }

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

        public TransfereeViewModel transferee { get; set; }

        public string Tenant { get; set; }
        public string LandLord { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal RentIncrease { get; set; }
        public decimal SecurityDeposit { get; set; }
        public string SecurityDepositTerms { get; set; }
        public string LeaseEndNoticeTerms { get; set; }
        public string RenewalTerms { get; set; }
        public string DiplomatTerms { get; set; }
        public string EarlyTerminationTerms { get; set; }
        public string NotableClauses { get; set; }
        public string PaymentInformation { get; set; }
        public decimal InitialRentAmount { get; set; }
        public DateTime? InitialRentDueDate { get; set; }
        public string InitialRentPaideTo { get; set; }
        public DateTime? SecurityDepositDueDate { get; set; }
        public string SecurityDepositPaideTo { get; set; }
        public decimal FirstOnGoingRentAmount { get; set; }
        public DateTime? FirstOnGoingRentDueDate { get; set; }
        public string FirstOnGoingRentPaideTo { get; set; }

    }
}
