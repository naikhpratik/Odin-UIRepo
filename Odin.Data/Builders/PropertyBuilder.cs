using Bogus;
using Odin.Data.Core.Models;

namespace Odin.Data.Builders
{
    public class PropertyBuilder
    {
        public static Property New()
        {
            var property = new Faker<Property>()
                .RuleFor(p => p.Street1, f => f.Address.StreetAddress())
                .RuleFor(p => p.Street2, f => f.Address.SecondaryAddress())
                .RuleFor(p => p.City, f => f.Address.City())
                .RuleFor(p => p.PostalCode, f => f.Address.ZipCode())
                .RuleFor(p => p.State, f => f.Address.StateAbbr())
                .RuleFor(p => p.CountryCode, "USA")

                // !!!: Can't figure out this one just yet
                //.RuleFor(r => r.NumberOfBathrooms, f => f.PickRandom<NumberOfBathroomsType>())
                .RuleFor(p => p.NumberOfBedrooms, f => f.Random.Int(1, 10))
                .RuleFor(p => p.SquareFootage, f => f.Random.Int(500, 4000))
                .RuleFor(p => p.Amount, f => f.Random.Decimal(500, 12000))
                .RuleFor(p => p.AvailabilityDate, f => f.Date.Future())
                .RuleFor(p => p.Description, f => f.Lorem.Paragraph());
            
            return property;
        }
    }
}
