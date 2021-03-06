﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Odin.Data.Core.Models
{
    public class HomeFinding : MobileTable
    {
        public HomeFinding()
        {
            HomeFindingProperties = new Collection<HomeFindingProperty>();
        }

        public decimal? HousingBudget { get; set; }
        //TODO: The rest of the Housing Preferences
        
        public Order Order { get; set; }

        public int? NumberOfBedrooms { get; set; }

        public NumberOfBathroomsType NumberOfBathrooms { get; set; }

        public byte? NumberOfBathroomsTypeId { get; set; }

        public int? SquareFootage { get; set; }
        
        public OwnershipType OwnershipType { get; set; }

        public HousingType HousingType  { get; set; }

        public byte? HousingTypeId { get; set; }

        public TransportationType TransportationType { get; set; }

        public byte? TransportationTypeId { get; set; }

        public bool? IsFurnished { get; set; }

        public int? MaxCommute { get; set; }

        public bool? HasParking { get; set; }

        public bool? HasLaundry { get; set; }

        public bool? HasAC { get; set; }

        public bool? HasExerciseRoom { get; set; }

        public string Comments { get; set; }

        public AreaType AreaType { get; set; }

        public byte? AreaTypeId { get; set; }

        public int? NumberOfCarsOwned { get; set; }

        public virtual ICollection<HomeFindingProperty> HomeFindingProperties { get; set; }
    }
}
