using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Odin.Data.Builders;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;
using Odin.IntegrationTests.TestAttributes;
using Odin.PropBotWebJob;
using Odin.PropBotWebJob.Interfaces;

namespace Odin.IntegrationTests.PropBotWebJob
{
    [TestFixture]
    public class FunctionsTest
    {
        private Functions _functions;
        private ApplicationDbContext _context;
        private Manager _pm;
        private Consultant _dsc;
        private Transferee _transferee;

        private Mock<IBotHelper> _mockBotHelper;
        private Mock<IBot> _mockBot;
        private Mock<TextWriter> _mockTextWriter;

        private string _url;

        [SetUp]
        public void SetUp()
        {
            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));

            var mapper = config.CreateMapper();

            _context = new ApplicationDbContext();
            _transferee = _context.Transferees.First(u => u.UserName.Equals("odinee@dwellworks.com"));
            _dsc = _context.Consultants.First(u => u.UserName.Equals("odinconsultant@dwellworks.com"));
            _pm = _context.Managers.First(u => u.UserName.Equals("odinpm@dwellworks.com"));


            _mockBotHelper = new Mock<IBotHelper>();
            _mockBot = new Mock<IBot>();
            _mockTextWriter = new Mock<TextWriter>();

            _url = "http://test.com";
            _mockBotHelper.Setup(b => b.GetBot(_url)).Returns(_mockBot.Object);

            _functions = new Functions(new UnitOfWork(_context), _mockBotHelper.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test, Isolated]
        public void ProcessQueueMessage_ValidPropertyAndImage_AddPropertyAndImage()
        {
            // Arrange
            Order order = OrderBuilder.New().First();
            order.ProgramManager = _pm;
            order.Transferee = _transferee;
            order.Consultant = _dsc;
            order.HomeFinding = new HomeFinding();

            _context.Orders.Add(order);
            _context.SaveChanges();
            _context.Entry(order).Reload();

            
            var property = PropertyBuilder.New();
            _mockBot.Setup(b => b.Bot()).Returns(property);

            var imageUrl = _url + "/image.jpg";
            _mockBot.Setup(b => b.BotImages()).Returns(new List<string> { imageUrl });

            Photo photo = new Photo()
            {
                Id = "1",
                StorageId = "1"
            };
            _mockBotHelper.Setup(bh => bh.SaveImageToStore(imageUrl)).Returns(photo);

            PropBotJobQueueEntry queueEntry = new PropBotJobQueueEntry()
            {
                OrderId = order.Id,
                PropertyUrl = _url,
                Notes = "Some Notes"
            };

            // Act
            _functions.ProcessQueueMessage(JsonConvert.SerializeObject(queueEntry), 0, _mockTextWriter.Object);

            // Assert
            _context.Entry(order).Reload();
            order.HomeFinding.Should().NotBeNull();
            order.HomeFinding.HomeFindingProperties.Count.Should().Be(1);
            order.HomeFinding.HomeFindingProperties.FirstOrDefault().Property.Id.Should().Be(property.Id);
            order.HomeFinding.HomeFindingProperties.FirstOrDefault().Property.Photos.Count.Should().Be(1);
        }

        [Test, Isolated]
        public void ProcessQueueMessage_ValidPropertyScrapeImageException_AddProperty()
        {
            // Arrange
            Order order = OrderBuilder.New().First();
            order.ProgramManager = _pm;
            order.Transferee = _transferee;
            order.Consultant = _dsc;
            order.HomeFinding = new HomeFinding();

            _context.Orders.Add(order);
            _context.SaveChanges();
            _context.Entry(order).Reload();


            var property = PropertyBuilder.New();
            _mockBot.Setup(b => b.Bot()).Returns(property);

            _mockBot.Setup(b => b.BotImages()).Throws<Exception>();

            PropBotJobQueueEntry queueEntry = new PropBotJobQueueEntry()
            {
                OrderId = order.Id,
                PropertyUrl = _url,
                Notes = "Some Notes"
            };

            // Act
            _functions.ProcessQueueMessage(JsonConvert.SerializeObject(queueEntry), 0, _mockTextWriter.Object);

            // Assert
            _context.Entry(order).Reload();
            order.HomeFinding.Should().NotBeNull();
            order.HomeFinding.HomeFindingProperties.Count.Should().Be(1);
            //Property should still get added, even if the image scrape fails.
            order.HomeFinding.HomeFindingProperties.FirstOrDefault().Property.Id.Should().Be(property.Id);
            order.HomeFinding.HomeFindingProperties.FirstOrDefault().Property.Photos.Count.Should().Be(0);
        }

        [Test, Isolated]
        public void ProcessQueueMessage_ValidPropertyValidImageBadImage_AddPropertyAndValidImage()
        {
            // Arrange
            Order order = OrderBuilder.New().First();
            order.ProgramManager = _pm;
            order.Transferee = _transferee;
            order.Consultant = _dsc;
            order.HomeFinding = new HomeFinding();

            _context.Orders.Add(order);
            _context.SaveChanges();
            _context.Entry(order).Reload();


            var property = PropertyBuilder.New();
            _mockBot.Setup(b => b.Bot()).Returns(property);

            var imageUrl = _url + "/image.jpg";
            var badImageUrl = _url + "/bad-image.jpg";
            _mockBot.Setup(b => b.BotImages()).Returns(new List<string> { imageUrl,badImageUrl });

            Photo photo = new Photo()
            {
                Id = "1",
                StorageId = "1"
            };
            _mockBotHelper.Setup(bh => bh.SaveImageToStore(imageUrl)).Returns(photo);
            _mockBotHelper.Setup(bh => bh.SaveImageToStore(badImageUrl)).Throws<Exception>();

            PropBotJobQueueEntry queueEntry = new PropBotJobQueueEntry()
            {
                OrderId = order.Id,
                PropertyUrl = _url,
                Notes = "Some Notes"
            };

            // Act
            _functions.ProcessQueueMessage(JsonConvert.SerializeObject(queueEntry), 0, _mockTextWriter.Object);

            // Assert
            _context.Entry(order).Reload();
            order.HomeFinding.Should().NotBeNull();
            order.HomeFinding.HomeFindingProperties.Count.Should().Be(1);
            order.HomeFinding.HomeFindingProperties.FirstOrDefault().Property.Id.Should().Be(property.Id);
            //Only the valid photo should be added.
            order.HomeFinding.HomeFindingProperties.FirstOrDefault().Property.Photos.Count.Should().Be(1);
        }

        [Test, Isolated]
        public void ProcessQueueMessage_BadProperty_PropertNotAdded()
        {
            // Arrange
            Order order = OrderBuilder.New().First();
            order.ProgramManager = _pm;
            order.Transferee = _transferee;
            order.Consultant = _dsc;
            order.HomeFinding = new HomeFinding();

            _context.Orders.Add(order);
            _context.SaveChanges();
            _context.Entry(order).Reload();

            var property = PropertyBuilder.New();
            _mockBot.Setup(b => b.Bot()).Throws<Exception>();

            PropBotJobQueueEntry queueEntry = new PropBotJobQueueEntry()
            {
                OrderId = order.Id,
                PropertyUrl = _url,
                Notes = "Some Notes"
            };

            // Act
            try
            {
                _functions.ProcessQueueMessage(JsonConvert.SerializeObject(queueEntry), 0, _mockTextWriter.Object);
            }
            catch (Exception e)
            {
                
            }

            // Assert
            _context.Entry(order).Reload();
            order.HomeFinding.Should().NotBeNull();
            order.HomeFinding.HomeFindingProperties.Count.Should().Be(0);
        }
    }
}
