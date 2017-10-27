using Bogus;
using Odin.Data.Core.Models;

namespace Odin.Data.Builders
{
    public static class ChildBuilder
    {
        public static Child New()
        {
            var child = new Faker<Child>()
                .RuleFor(t => t.Name, f => f.Name.FirstName())
                .RuleFor(t => t.Age, f => f.PickRandom<int>(new int[]{1,2,3,4,5,6,7,8,9,10,11,12}))
                .RuleFor(t => t.Grade, f => f.PickRandom<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }));

            return child;
        }
    }
}
