using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Odin.PropBotWebJob.Bots;

namespace Odin.PropBotWebJob.Tests.Bots
{
    [TestClass]
    public class CraigslistBotTests
    {
        private const string RENT_URL =
            "https://charlotte.craigslist.org/apa/d/2bd-25bath-uptown-townhome/6351485009.html";

        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockMapper = new Mock<IMapper>();
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\CraigslistRent.html", "BotHtml")]
        public void CraigslistBot_BotRentUrl_ShouldReturnProperty()
        {
            string url = RENT_URL;

            string html = File.ReadAllText("BotHtml\\CraigslistRent.html");
            CraigslistBot bot = new CraigslistBot(url, _mockMapper.Object, html);
            var property = bot.Bot();

            property.Should().NotBeNull();
            property.Amount.Should().Be(1600);
            property.SquareFootage.Should().Be(1200);
            property.NumberOfBedrooms.Should().Be(2);
            property.NumberOfBathrooms.Should().Be(2.5m);
            property.SourceUrl.Should().Be(url);
            property.Latitude.Should().Be(35.234644m);
            property.Longitude.Should().Be(-80.853613m);
        }

        [TestMethod]
        [DeploymentItem(@"BotHtml\\CraigslistRent.html", "BotHtml")]
        public void CraigslistBot_BotRentImages_ShouldReturnImageUrls()
        {
            string url = RENT_URL;

            string html = File.ReadAllText("BotHtml\\CraigslistRent.html");
            CraigslistBot bot = new CraigslistBot(url, _mockMapper.Object, html);
            var images = bot.BotImages();

            images.Should().NotBeNull();
            images.Count().Should().Be(12);
        }
    }
}
