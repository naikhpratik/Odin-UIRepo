using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Odin.Data.Core.Dtos;

namespace Odin.Data.Builders
{
    public static class ConsultantImportDtoBuilder
    {
        public static IList<ConsultantImportDto> New(int count = 1)
        {
            var consultantDto = new Faker<ConsultantImportDto>()
                .RuleFor(t => t.Email, f => f.Internet.Email())
                .RuleFor(t => t.FirstName, f => f.Name.FirstName())
                .RuleFor(t => t.LastName, f => f.Name.LastName())
                .RuleFor(t => t.SeContactUid, f => f.IndexFaker + 1);

            var consultants = consultantDto.Generate(count);

            return consultants;
        }
    }
}
