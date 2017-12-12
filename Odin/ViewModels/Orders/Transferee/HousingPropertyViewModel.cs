﻿using System;
using Odin.Data.Core.Models;
using Odin.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web;
using System.Collections.ObjectModel;
using Odin.ViewModels.Shared;
using System.Linq;

namespace Odin.ViewModels.Orders.Transferee
{
    public class HousingPropertyViewModel
    {
        public HousingPropertyViewModel()
        {
            UploadedPhotos = new Collection<HttpPostedFileBase>();
        }

        public bool Deleted { get; set; }

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
        [Required(ErrorMessage = "Street 1 is required")]
        public String PropertyStreet1 { get; set; }

        [Display(Name = "Street 2")]
        public String PropertyStreet2 { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required")]
        public String PropertyCity { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "State is required")]
        [MaxLength(2)]
        [DataType("UnitedStates")]
        public String PropertyState { get; set; }

        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "Postal Code is required")]
        [DataType(DataType.PostalCode)]
        public String PropertyPostalCode { get; set; }

        [Display(Name = "Availability Date")]
        [DisplayFormat(NullDisplayText = "Unknown", DataFormatString = "{0:dd-MMM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PropertyAvailabilityDate { get; set; }

        [Display(Name = "Beds")]
        [DisplayFormat(NullDisplayText = "NA")]
        public int? PropertyNumberOfBedrooms { get; set; }

        [Display(Name = "Baths")]
        [RegularExpression(@"^(\d{0,2})(.{0,1})([0,5]{0,1})$", ErrorMessage = "Baths must be in increments of .5")]
        [DisplayFormat(NullDisplayText = "NA", DataFormatString = "{0:N1}", ApplyFormatInEditMode = true)]
        public decimal? PropertyNumberOfBathrooms { get; set; }

        [Display(Name = "Sq. Footage")]
        [DisplayFormat(NullDisplayText = "NA")]
        [Range(0, 99999)]
        public int? PropertySquareFootage { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(NullDisplayText = "NA", DataFormatString = "{0:c}")]
        [DataType(DataType.Currency)]
        public Decimal PropertyAmount { get; set; }

        [Display(Name = "Description/Notes:")]
        [DataType(DataType.MultilineText)]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public String PropertyDescription { get; set; }

        [Display(Name = "Photos")]
        public IEnumerable<HttpPostedFileBase> UploadedPhotos { get; set; }

        [DataType("Photos")]
        public IEnumerable<PhotoViewModel> PropertyPhotos { get; set; }

        public String ThumbnailPhotoUrl {  get
            {
                String thumbUrl = "/Content/Images/popout_sml_img.png";

                if (PropertyPhotos.Any())
                {
                    thumbUrl = PropertyPhotos.ElementAt(0).PhotoUrl;
                }

                return thumbUrl;
            }
        }

        [DataType("LikeDislike")]
        public bool? Liked { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? ViewingDate { get; set; }
    }
}
