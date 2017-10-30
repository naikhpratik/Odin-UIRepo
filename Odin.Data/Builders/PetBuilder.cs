using Bogus;
using Odin.Data.Core.Models;

namespace Odin.Data.Builders
{
    public class PetBuilder
    {
        public static Pet New()
        {
            var pet = new Faker<Pet>()
                .RuleFor(t => t.Type, f => f.PickRandom<string>(new string[]{"Dog","Cat","Fish","Snake"}))
                .RuleFor(t => t.Breed, f => f.PickRandom<string>(new string[] {"German","Chinese","Japanese","English","French"} ))
                .RuleFor(t => t.Size, f => f.PickRandom<string>(new string[] { "Small","Medium","Large" }));

            return pet;
        }
    }
}
