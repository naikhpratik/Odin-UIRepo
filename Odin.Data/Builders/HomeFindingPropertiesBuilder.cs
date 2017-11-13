using Bogus;
using Odin.Data.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Odin.Data.Builders
{
    public class HomeFindingPropertiesBuilder
    {
        public static List<HomeFindingProperty> New(int count = 1)
        {
            var homeFindingPropertyGenerator = new Faker<HomeFindingProperty>()
                .RuleFor(p => p.Property, PropertyBuilder.New());

            var homeFindingProperties = homeFindingPropertyGenerator.Generate(count).ToList();

            return homeFindingProperties;
        }
    }
}
