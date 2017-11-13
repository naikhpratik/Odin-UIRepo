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
                .RuleFor(r => r.MaxCommute, f => f.Random.Int(10, 60))
                .RuleFor(r => r.Comments, f => f.Lorem.Paragraph())
                .RuleFor(r => r.NumberOfCarsOwned, f => f.Random.Int(1, 10))
                .RuleFor(r => r.IsFurnished, f => f.Random.Bool())
                .RuleFor(r => r.HasParking, f => f.Random.Bool())
                .RuleFor(r => r.HasLaundry, f => f.Random.Bool())
                .RuleFor(r => r.HasAC, f => f.Random.Bool())
                .RuleFor(r => r.HasExerciseRoom, f => f.Random.Bool())
                .RuleFor(r => r.HomeFindingProperties, HomeFindingPropertiesBuilder.New());

            return homeFinding;
        }
    }
}
