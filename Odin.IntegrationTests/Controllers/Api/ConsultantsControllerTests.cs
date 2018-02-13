using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Builders;
using Odin.Data.Core.Dtos;
using Odin.IntegrationTests.TestAttributes;

namespace Odin.IntegrationTests.Controllers.Api
{   
    //Incomplete Test//
    [TestFixture]
    public class ConsultantsControllerTests : WebApiBaseTest
    {
        [Test, CleanData]
        public async Task UpsertConsultants_ValidUpdateRequest_ShouldUpdateConsultants()
        {
            // Arrange
            var consultantImportDto = ConsultantImportDtoBuilder.New().First();
            consultantImportDto.SeContactUid = dsc.SeContactUid.Value;
            var consultantsImportDto = new ConsultantsDto();
            consultantsImportDto.Consultants = new List<ConsultantImportDto>() { consultantImportDto};

            // Act
            var request = CreateRequest("api/consultants", "application/json", HttpMethod.Post, consultantsImportDto);

        }
        //UpsertConsultants_ValidInsertRequest_ShouldInsertConsultants
        //UpsertConsultants_ValidRequest_ShouldUpdateAndInsertConsultants
    }
}
