using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Odin.Data.Core.Dtos;

namespace Odin.Data.Builders
{
    public static class ConsultantDtoBuilder
    {
        public static ConsultantDto New()
        {
            var consultantDto = new Faker<ConsultantDto>()
                .RuleFor(t => t.Email, f => f.Internet.Email())
                .RuleFor(t => t.FirstName, f => f.Name.FirstName())
                .RuleFor(t => t.LastName, f => f.Name.LastName())
                .RuleFor(t => t.SeContactUid, f => f.IndexFaker + 1);

            return consultantDto;
        }
    }
}
