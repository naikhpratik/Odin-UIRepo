﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Odin.Data.Core.Models;

namespace Odin.Data.Builders
{
    public class RentBuilder
    {
        public static Rent New()
        {
            
            var rent = new Faker<Rent>()
                .RuleFor(r => r.NumberOfBedrooms, f => f.Random.Int(1,10))
                .RuleFor(r => r.NumberOfBathrooms, f => f.Random.Int(1,10))
                .RuleFor(r => r.HousingBudget, f=> f.Random.Decimal(500,12000))
                .RuleFor(r => r.SquareFootage, f=> f.Random.Int(500,4000))
                .RuleFor(r => r.OwnershipType, f => f.PickRandom<OwnershipType>())
                .RuleFor(r => r.HousingType, f => f.PickRandom<HousingType>());

            return rent;
        }
    }
}