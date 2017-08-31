using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Odin.Data.Core.Models;

namespace Odin.Data.Builders
{
    public static class TransfereeBuilder
    {
        public static IList<Transferee> New(int count = 1)
        {
            var transferee = new Faker<Transferee>()
                .RuleFor(t => t.Email, f => f.Internet.Email())
                .RuleFor(t => t.FirstName, f => f.Name.FirstName())
                .RuleFor(t => t.LastName, f => f.Name.LastName())
                .RuleFor(t => t.SpouseName, f=> f.Name.FullName());

            var transferees = transferee.Generate(count);

            return transferees;
        }
    }
}
