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
    public class ApartmentsBotTest
    {
        private const string APARTMENT_URL = "https://www.apartments.com/the-edison-at-gordon-square-cleveland-oh/hb3zmw0/";
        private const string HOUSE_URL = "https://www.apartments.com/16409-throckley-ave-cleveland-oh/7hgly8y/";
        private const string BAD_URL = "https://www.apartments.com";

        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockMapper = new Mock<IMapper>();
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\ApartmentsApartment.html", "BotHtml")]
        public void ApartmentBot_BotApartmentUrl_ShouldReturnProperty()
        {
            string url = APARTMENT_URL;

            string html = File.ReadAllText("BotHtml\\ApartmentsApartment.html");
            ApartmentsBot bot = new ApartmentsBot(url, _mockMapper.Object, html);
            var property = bot.Bot();

            property.Should().NotBeNull();
            property.Latitude.Should().Be(41.48965m);
            property.Longitude.Should().Be(-81.72891m);
            property.NumberOfBathrooms.Should().Be(1);
            property.NumberOfBedrooms.Should().Be(1);
            property.City.Should().Be("Cleveland");
            property.State.Should().Be("OH");
            property.PostalCode.Should().Be("44102");
            property.Street1.Should().Be("6060 Father Caruso");
            property.Description.Should().NotBeNullOrWhiteSpace();
            property.Amount.Should().Be(1370);
            property.SquareFootage.Should().Be(561);
            property.SourceUrl.Should().Be(url);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\ApartmentsHouse.html", "BotHtml")]
        public void ApartmentBot_BotHouseUrl_ShouldReturnProperty()
        {
            string url = HOUSE_URL;

            string html = File.ReadAllText("BotHtml\\ApartmentsHouse.html");
            ApartmentsBot bot = new ApartmentsBot(url, _mockMapper.Object, html);
            var property = bot.Bot();

            property.Should().NotBeNull();
            property.Latitude.Should().Be(41.45612m);
            property.Longitude.Should().Be(-81.56556m);
            property.NumberOfBathrooms.Should().Be(1);
            property.NumberOfBedrooms.Should().Be(2);
            property.City.Should().Be("Cleveland");
            property.State.Should().Be("OH");
            property.PostalCode.Should().Be("44128");
            property.Street1.Should().Be("16409 Throckley Ave");
            property.Description.Should().NotBeNullOrWhiteSpace();
            property.Amount.Should().Be(700);
            property.SquareFootage.Should().Be(894);
            property.SourceUrl.Should().Be(url);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\ApartmentsApartment.html", "BotHtml")]
        public void ApartmentsBot_BotApartmentUrlImages_ShouldReturnImages()
        {
            string url = APARTMENT_URL;

            string html = File.ReadAllText("BotHtml\\ApartmentsApartment.html");
            ApartmentsBot bot = new ApartmentsBot(url, _mockMapper.Object, html);
            var images = bot.BotImages();

            images.Should().NotBeNull();
            images.Count().Should().Be(13);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\ApartmentsHouse.html", "BotHtml")]
        public void ApartmentsBot_BotHouseUrlImages_ShouldReturnImages()
        {
            string url = HOUSE_URL;

            string html = File.ReadAllText("BotHtml\\ApartmentsHouse.html");
            ApartmentsBot bot = new ApartmentsBot(url, _mockMapper.Object, html);
            var images = bot.BotImages();

            images.Should().NotBeNull();
            images.Count().Should().Be(20);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\ApartmentsBad.html", "BotHtml")]
        [ExpectedException(typeof(NullReferenceException))]
        public void ApartmentsBot_BotBadUrl_ShouldThrowException()
        {
            string url = BAD_URL;
            string html = File.ReadAllText("BotHtml\\ApartmentsBad.html");

            ApartmentsBot bot = new ApartmentsBot(url, _mockMapper.Object, html);
            bot.Bot();
        }

    }
}
