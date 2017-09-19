using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;

namespace Odin.Data.Builders
{
    public static class TransfereeDtoBuilder
    {
        public static TransfereeDto New()
        {
            var transferee = new Faker<TransfereeDto>()
                .RuleFor(t => t.Email, f => f.Internet.Email())
                .RuleFor(t => t.FirstName, f => f.Name.FirstName())
                .RuleFor(t => t.LastName, f => f.Name.LastName());

            return transferee;
        }
    }
}
