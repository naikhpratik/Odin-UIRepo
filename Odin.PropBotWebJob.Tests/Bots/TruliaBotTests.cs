using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.Data.Core.Models;
using Odin.PropBotWebJob.Bots;
using Odin.PropBotWebJob.Dtos.Trulia;
using System;
using System.IO;
using System.Linq;

namespace Odin.PropBotWebJob.Tests.Bots
{
    [TestClass]
    public class TruliaBotTests
    {
        private const string RENTAL_URL =
                "https://www.trulia.com/rental-community/9000022703/Hunter-s-Chase-Apartments-1575-Hunters-Chase-Dr-Westlake-OH-44145/";
   
        private const string PROPERTY_URL =
            "https://www.trulia.com/property/5033371762-6411-Springwood-Rd-Cleveland-OH-44130";

        private const string BUILDER_COMMUNITY_URL = 
            "https://www.trulia.com/builder-community/Battery-Park-3276569609/?omni_src=builder|promotedcommunitiesbanner";

        private const string BAD_URL = "https://www.trulia.com";

        private const string BAD_BOT_TYPE_URL = "https://www.trulia.com/bad/Battery-Park-3276569609/?omni_src=builder|promotedcommunitiesbanner";


        private Mock<IMapper> _mockMapper;
        private IMapper _mapper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockMapper = new Mock<IMapper>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TruliaRentDto, Property>();
                cfg.CreateMap<TruliaBuyFeatureDto, Property>();
                cfg.CreateMap<TruliaBuyLocationDto, Property>();
                cfg.CreateMap<TruliaBuyPriceDto, Property>();
            });

            _mapper = config.CreateMapper();
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\TruliaRental.html", "BotHtml")]
        public void TruliaBot_BotRentalJson_ShouldReturnProperty()
        {
            string url = RENTAL_URL;
               
            string html = File.ReadAllText("BotHtml\\TruliaRental.html");
            TruliaBot bot = new TruliaBot(url,_mapper,html);
            var property = bot.Bot();

            property.Should().NotBeNull();
            property.Latitude.Should().Be(41.466267m);
            property.Longitude.Should().Be(-81.94102m);
            property.NumberOfBathrooms.Should().Be(1);
            property.NumberOfBedrooms.Should().Be(2);
            property.City.Should().Be("Westlake");
            property.State.Should().Be("OH");
            property.PostalCode.Should().Be("44145");
            property.Street1.Should().Be("1575 Hunters Chase Dr");
            property.Description.Should().NotBeNullOrWhiteSpace();
            property.Amount.Should().Be(909);
            property.SquareFootage.Should().Be(676);
            property.SourceUrl.Should().Be(url);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\TruliaRental.html", "BotHtml")]
        public void TruliaBot_BotRental_ShouldReturnProperty()
        {
            string url = RENTAL_URL;
            string html = File.ReadAllText("BotHtml\\TruliaRental.html");

            //Mock mapper will cause an exception to be thrown in BotRentalJson and cause BotRental to fire.
            TruliaBot bot = new TruliaBot(url, _mockMapper.Object, html);
            var property = bot.Bot();

            property.Should().NotBeNull();
            property.NumberOfBathrooms.Should().Be(1);
            property.NumberOfBedrooms.Should().Be(1);
            property.City.Should().Be("Westlake".ToUpper());
            property.State.Should().Be("OH");
            property.PostalCode.Should().Be("44145");
            property.Street1.Should().Be("1575 Hunters Chase Dr".ToUpper());
            property.Description.Should().NotBeNullOrWhiteSpace();
            property.Amount.Should().Be(909);
            property.SourceUrl.Should().Be(url);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\TruliaProperty.html", "BotHtml")]
        public void TruliaBot_BotPropertyJson_ShouldReturnBotProperty()
        {
            string url = PROPERTY_URL;
                
            string html = File.ReadAllText("BotHtml\\TruliaProperty.html");
            TruliaBot bot = new TruliaBot(url, _mapper, html);
            var property = bot.Bot();

            property.Should().NotBeNull();
            property.Latitude.Should().Be(41.38862m);
            property.Longitude.Should().Be(-81.74691m);
            property.NumberOfBathrooms.Should().Be(1);
            property.NumberOfBedrooms.Should().Be(3);
            property.City.Should().Be("Cleveland");
            property.State.Should().Be("OH");
            property.PostalCode.Should().Be("44130");
            property.Street1.Should().Be("6411 Springwood Rd");
            property.Description.Should().NotBeNullOrWhiteSpace();
            property.Amount.Should().Be(126900);
            property.SquareFootage.Should().Be(1386);
            property.SourceUrl.Should().Be(url);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\TruliaProperty.html", "BotHtml")]
        public void TruliaBot_BotProperty_ShouldReturnProperty()
        {
            string url = PROPERTY_URL;
            string html = File.ReadAllText("BotHtml\\TruliaProperty.html");

            TruliaBot bot = new TruliaBot(url, _mockMapper.Object, html);
            var property = bot.Bot();

            property.Should().NotBeNull();
            property.NumberOfBathrooms.Should().Be(1);
            property.NumberOfBedrooms.Should().Be(3);
            property.City.Should().Be("Cleveland".ToUpper());
            property.State.Should().Be("OH");
            property.PostalCode.Should().Be("44130");
            property.Street1.Should().Be("6411 Springwood Rd".ToUpper());
            property.Description.Should().NotBeNullOrWhiteSpace();
            property.Amount.Should().Be(126900);
            property.SquareFootage.Should().Be(1386);
            property.SourceUrl.Should().Be(url);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\TruliaBuilderCommunity.html", "BotHtml")]
        public void TruliaBot_BotBuilderCommunity_ShouldReturnProperty()
        {
            string url = BUILDER_COMMUNITY_URL;
            string html = File.ReadAllText("BotHtml\\TruliaBuilderCommunity.html");

            TruliaBot bot = new TruliaBot(url, _mapper, html);
            var property = bot.Bot();

            property.Should().NotBeNull();
            property.Latitude.Should().Be(41);
            property.Longitude.Should().Be(-81);
            property.NumberOfBathrooms.Should().Be(2);
            property.NumberOfBedrooms.Should().Be(2);
            property.City.Should().Be("Cleveland");
            property.State.Should().Be("OH");
            property.PostalCode.Should().Be("44102");
            property.Street1.Should().Be("7524 Father Frascati Dr");
            property.Description.Should().NotBeNullOrWhiteSpace();
            property.Amount.Should().Be(499900);
            property.SquareFootage.Should().Be(1817);
            property.SourceUrl.Should().Be(url);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\TruliaRental.html", "BotHtml")]
        public void TruliaBot_BotRentalImages_ShouldReturnImages()
        {
            string url = RENTAL_URL;
            string html = File.ReadAllText("BotHtml\\TruliaRental.html");

            TruliaBot bot = new TruliaBot(url, _mapper, html);
            var images = bot.BotImages();

            images.Should().NotBeNull();
            images.Count().Should().Be(35);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\TruliaProperty.html", "BotHtml")]
        public void TruliaBot_BotBuyImages_ShouldReturnImages()
        {
            string url = PROPERTY_URL;
            string html = File.ReadAllText("BotHtml\\TruliaProperty.html");

            TruliaBot bot = new TruliaBot(url, _mapper, html);
            var images = bot.BotImages();

            images.Should().NotBeNull();
            images.Count().Should().Be(35);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TruliaBot_BadUrl_ShouldThrowOutOfRangeException()
        {
            string url = BAD_URL;
            string html = File.ReadAllText("BotHtml\\TruliaProperty.html");

            TruliaBot bot = new TruliaBot(url, _mapper, html);
            bot.Bot();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Unknown bot type!")]
        public void TruliaBot_BadBotTypeUrl_ShouldThrowException()
        {
            string url = BAD_BOT_TYPE_URL;
            string html = File.ReadAllText("BotHtml\\TruliaProperty.html");

            TruliaBot bot = new TruliaBot(url, _mapper, html);
            bot.Bot();
        }
    }
}
