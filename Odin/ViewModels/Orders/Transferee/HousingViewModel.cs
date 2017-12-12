using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Odin.ViewModels.Shared;
using System.ComponentModel.DataAnnotations;
using Odin.Data.Core.Models;
using AutoMapper;

namespace Odin.ViewModels.Orders.Transferee
{
    public class HousingViewModel
    {   
        public HousingViewModel(Order order, IMapper mapper) : this()
        {

            mapper.Map<HomeFinding, HousingViewModel>(order.HomeFinding, this);
            mapper.Map<Order, HousingViewModel>(order, this);

            IEnumerable<HomeFindingProperty> homeFindingProperties = order.HomeFinding.HomeFindingProperties.Where(hfp => !hfp.Deleted).OrderByDescending(hfp => hfp.CreatedAt);
            IEnumerable<HousingPropertyViewModel> propertyViewModels;
            propertyViewModels = mapper.Map<IEnumerable<HomeFindingProperty>, IEnumerable<HousingPropertyViewModel>>(homeFindingProperties);

            this.Properties = propertyViewModels;
        }
        //this version will replace the above constructor when the viewings portion is completed: 
        //the isViewing flag will detrmine if only those properties that have a viewing scheduled will be returned
        public HousingViewModel(Order order, IMapper mapper, string listChoice) : this()
        {
           
            mapper.Map<HomeFinding, HousingViewModel>(order.HomeFinding, this);
            mapper.Map<Order, HousingViewModel>(order, this);

            IEnumerable<HomeFindingProperty> homeFindingProperties;         
            
            //list choices: AllProperties, ViewingsOnly, or NoViewings   
            //filters to be applied when home viewing module is completed. The only active choice is "AllProperties"
            //ViewingsOnly
            if (listChoice == "ViewingsOnly")
            {
                homeFindingProperties = order.HomeFinding.HomeFindingProperties.Where(hfp => !hfp.Deleted && hfp.ViewingDate != null).OrderByDescending(hfp => hfp.CreatedAt);
            }
            //NoViewings
            else if (listChoice == "NoViewings")
            {
                homeFindingProperties = order.HomeFinding.HomeFindingProperties.Where(hfp => !hfp.Deleted && hfp.ViewingDate == null).OrderByDescending(hfp => hfp.CreatedAt);
            }
            else
            //AllProperties
                homeFindingProperties = order.HomeFinding.HomeFindingProperties.Where(hfp => !hfp.Deleted).OrderByDescending(hfp => hfp.CreatedAt);

            IEnumerable<HousingPropertyViewModel> propertyViewModels;
            propertyViewModels = mapper.Map<IEnumerable<HomeFindingProperty>, IEnumerable<HousingPropertyViewModel>>(homeFindingProperties);

            this.Properties = propertyViewModels;
        }
        public HousingViewModel()
        {
            Properties = new List<HousingPropertyViewModel>();
        }

        public string Id { get; set; }

        [Display(Name = "Budget:")]
        [DisplayFormat(DataFormatString = "{0:c}", NullDisplayText = "No Preference")]
        public decimal? HousingBudget { get; set; }

        [Display(Name = "Bedrooms:")]
        [DisplayFormat(NullDisplayText = "No Preference")]
        public int? NumberOfBedrooms { get; set; }

        [Display(Name = "Bathrooms:")]
        [DisplayFormat(NullDisplayText = "No Preference")]
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
        [DisplayFormat(NullDisplayText = "No Preference")]
        public int? MaxCommute { get; set; }

        [Display(Name = "Transportation:")]
        [DisplayFormat(NullDisplayText = "No Preference")]
        public String TransportationTypeName { get; set; }

        public IEnumerable<HousingPropertyViewModel> Properties { get; set; }
    }
}
