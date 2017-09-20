using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Odin.Data.Core.Dtos;

namespace Odin.Data.Builders
{
    public static class ProgramManagerDtoBuilder
    {
        public static ProgramManagerDto New()
        {
            var programManagerDto = new Faker<ProgramManagerDto>()
                .RuleFor(t => t.SeContactUid, f => f.IndexFaker + 1);

            return programManagerDto;
        }
    }
}
