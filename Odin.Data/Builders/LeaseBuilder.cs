using Bogus;
using Odin.Data.Core.Models;

namespace Odin.Data.Builders
{
    public static class LeaseBuilder
    {
        public static Lease New()
        {
            Lease lease = new Faker<Lease>()
            .RuleFor(l => l.LeaseTerm, f => f.PickRandom(new int[]{1,2,3,4,5,6,7,8,9,10,11,12}))
            .RuleFor(l => l.LengthOfAssignment, f => f.PickRandom(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }))

            return new Lease();
        }
    }
}
