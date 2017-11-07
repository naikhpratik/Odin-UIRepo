using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Odin.Data.Core.Models;

namespace Odin.Data.Builders
{
    public class HomeFindingBuilder
    {
        public static HomeFinding New()
        {

            var homeFinding = new Faker<HomeFinding>()
                .RuleFor(r => r.NumberOfBedrooms, f => f.Random.Int(1, 10))
                .RuleFor(r => r.HousingBudget, f => f.Random.Decimal(500, 12000))
                .RuleFor(r => r.SquareFootage, f => f.Random.Int(500, 4000))
                .RuleFor(r => r.OwnershipType, f => f.PickRandom<OwnershipType>())
                .RuleFor(r => r.HomeFindingProperties, HomeFindingPropertiesBuilder.New());

            return homeFinding;
        }
    }
}
