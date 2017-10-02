using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Odin.Data.Builders;
using Odin.Data.Core.Dtos;
using Odin.Data.Helpers;
using Odin.Data.Persistence;
using Odin.Domain;
using Odin.IntegrationTests.TestAttributes;

namespace Odin.IntegrationTests.Domain
{
    [TestFixture]
    public class ConsultantImporterTests
    {
        private ConsultantImporter _importer;
        private ApplicationDbContext _context;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));

            var mapper = config.CreateMapper();
            _context = new ApplicationDbContext();
            _importer = new ConsultantImporter(new UnitOfWork(_context), mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test, Isolated]
        public async Task ImportConsultants_ExistingConsultant_DoesNotAddOrUpdateConsultant()
        {
            // Arrange
            var dsc = _context.Consultants.First(u => u.UserName.Equals("odinconsultant@dwellworks.com"));
            var consultantCount = _context.Consultants.Count();
            var consultantImportDto = ConsultantImportDtoBuilder.New().First();
            consultantImportDto.SeContactUid = dsc.SeContactUid.Value;
            var consultantsImportDto =
                new ConsultantsDto {Consultants = new List<ConsultantImportDto>() {consultantImportDto}};

            // Act
            _importer.ImportConsultants(consultantsImportDto);

            // Assert
            _context.Entry(dsc).Reload();
            dsc.LastName.Should().NotBe(consultantImportDto.LastName);
            var consultantPostCount = _context.Consultants.Count();
            consultantPostCount.Should().Be(consultantCount);
        }
    }
}
