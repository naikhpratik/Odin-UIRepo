using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.PropBotWebJob.Bots;
using System;
using System.IO;
using System.Linq;

namespace Odin.PropBotWebJob.Tests.Bots
{
    [TestClass]
    public class RealtorBotTests
    {
        private const string BUY_URL = "https://www.realtor.com/realestateandhomes-detail/17453-Woodford-Ave_Lakewood_OH_44107_M48724-73142";
        private const string RENT_URL = "https://www.realtor.com/realestateandhomes-detail/1750-Euclid-Ave_Cleveland_OH_44115_M49592-48003";
        private const string BAD_URL = "https://www.realtor.com";

        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockMapper = new Mock<IMapper>();
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\RealtorBuy.html", "BotHtml")]
        public void RealtorBot_BotBuyUrl_ShouldReturnProperty()
        {
            string url = BUY_URL;

            string html = File.ReadAllText("BotHtml\\RealtorBuy.html");
            RealtorBot bot = new RealtorBot(url,_mockMapper.Object,html);
            var property = bot.Bot();

            property.Should().NotBeNull();
            property.Latitude.Should().Be(41.488898m);
            property.Longitude.Should().Be(-81.818954m);
            property.NumberOfBathrooms.Should().Be(2);
            property.NumberOfBedrooms.Should().Be(3);
            property.City.Should().Be("Lakewood");
            property.State.Should().Be("OH");
            property.PostalCode.Should().Be("44107");
            property.Street1.Should().Be("17453 Woodford Ave");
            property.Description.Should().NotBeNullOrWhiteSpace();
            property.Amount.Should().Be(130000);
            property.SquareFootage.Should().Be(1284);
            property.SourceUrl.Should().Be(url);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\RealtorRent.html", "BotHtml")]
        public void RealtorBot_BotRentUrl_ShouldReturnProperty()
        {
            string url = RENT_URL;

            string html = File.ReadAllText("BotHtml\\RealtorRent.html");
            RealtorBot bot = new RealtorBot(url, _mockMapper.Object, html);
            var property = bot.Bot();

            property.Should().NotBeNull();
            property.Latitude.Should().Be(41.50083m);
            property.Longitude.Should().Be(-81.67892m);
            property.NumberOfBathrooms.Should().Be(1);
            property.NumberOfBedrooms.Should().Be(0);
            property.City.Should().Be("Cleveland");
            property.State.Should().Be("OH");
            property.PostalCode.Should().Be("44115");
            property.Street1.Should().Be("1750 Euclid Ave");
            property.Description.Should().NotBeNullOrWhiteSpace();
            property.Amount.Should().Be(675);
            property.SquareFootage.Should().Be(492);
            property.SourceUrl.Should().Be(url);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\RealtorBuy.html", "BotHtml")]
        public void RealtorBot_BotBuyUrlImages_ShouldReturnImages()
        {
            string url = BUY_URL;

            string html = File.ReadAllText("BotHtml\\RealtorBuy.html");
            RealtorBot bot = new RealtorBot(url, _mockMapper.Object, html);
            var images = bot.BotImages();

            images.Should().NotBeNull();
            images.Count().Should().Be(35);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\RealtorRent.html", "BotHtml")]
        public void RealtorBot_BotRentUrlImages_ShouldReturnImages()
        {
            string url = RENT_URL;

            string html = File.ReadAllText("BotHtml\\RealtorRent.html");
            RealtorBot bot = new RealtorBot(url, _mockMapper.Object, html);
            var images = bot.BotImages();

            images.Should().NotBeNull();
            images.Count().Should().Be(35);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\RealtorBad.html", "BotHtml")]
        [ExpectedException(typeof(NullReferenceException))]
        public void RealtorBot_BotBadUrl_ShouldThrowException()
        {
            string url = BAD_URL;
            string html = File.ReadAllText("BotHtml\\RealtorBad.html");

            RealtorBot bot = new RealtorBot(url, _mockMapper.Object, html);
            bot.Bot();
        }
    }
}
