using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Odin.Data.Core.Models;

namespace Odin.Data.Builders
{
    public static class ConsultantBuilder
    {
        public static Consultant New()
        {
            var newUser = new Faker<Consultant>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.PhoneNumber, f => f.Person.Phone);

            return newUser.Generate();
        }
    }
}
