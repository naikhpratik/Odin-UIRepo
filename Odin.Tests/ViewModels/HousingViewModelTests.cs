using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Odin.Data.Core.Models;
using AutoMapper;
using Odin.Data.Builders;
using Odin.ViewModels.Orders.Transferee;
using FluentAssertions;
using System.Collections.ObjectModel;

namespace Odin.Tests.ViewModels
{
    [TestClass]
    public class HousingViewModelTests
    {
        private Order order;
        private IMapper mapper;

        [TestInitialize]
        public void TestInit ()
        {
            var orderId = "1";
            order = new Order() { Id = orderId };
            order.HomeFinding = new HomeFinding();

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            mapper = config.CreateMapper();
        }

        [TestMethod]
        public void TestSpouseNameFromOrder()
        {
            var spouseName = "My Great Name";
            order.SpouseName = spouseName;

            HousingViewModel viewModel = mapper.Map<Order, HousingViewModel>(order);

            (viewModel.SpouseName).Should().BeEquivalentTo(spouseName);
        }

        [TestMethod]
        public void TestChildCountFromOrder()
        {
            int numberOfTestChildren = 4;
            for (int i = 0; i < numberOfTestChildren; i++)
            {
                order.Children.Add(new Child());
            }

            HousingViewModel viewModel = mapper.Map<Order, HousingViewModel>(order);

            (viewModel.ChildrenCount).Should().Be(numberOfTestChildren);
        }

        [TestMethod]
        public void TestDisplayForSpouseAndNoKids()
        {
            order.SpouseName = "Some Name";

            HousingViewModel viewModel = mapper.Map<Order, HousingViewModel>(order);

            (viewModel.SpouseAndKids).Should().Be("Yes / 0");
        }

        [TestMethod]
        public void TestDisplayForSpouseAndFiveKids()
        {
            order.SpouseName = "Some Name";
            int numberOfTestChildren = 5;
            for (int i = 0; i < numberOfTestChildren; i++)
            {
                order.Children.Add(new Child());
            }

            HousingViewModel viewModel = mapper.Map<Order, HousingViewModel>(order);

            (viewModel.SpouseAndKids).Should().Be("Yes / 5");
        }

        [TestMethod]
        public void TestDisplayForNoFamily()
        {
            HousingViewModel viewModel = mapper.Map<Order, HousingViewModel>(order);

            (viewModel.SpouseAndKids).Should().Be(null);
        }

        [TestMethod]
        public void TestNumberOfPets()
        {
            int numberOfTestPets = 5;
            for (int i = 0; i < numberOfTestPets; i++)
            {
                order.Pets.Add(new Pet());
            }
            HousingViewModel viewModel = mapper.Map<Order, HousingViewModel>(order);

            (viewModel.PetsCount).Should().Be(numberOfTestPets);
        }

        [TestMethod]
        public void TestZeroPetsReturnsNull()
        {
            HousingViewModel viewModel = new HousingViewModel();
            viewModel.PetsCount = 0;

            (viewModel.PetsCount).Should().Be(null);
        }

        [TestMethod]
        public void TestDisplayForHousingType()
        {
            String housingTypeName = "My Great Housing Type";
            HousingType housingType = new HousingType { Name = housingTypeName };
            order.HomeFinding.HousingType = housingType;

            HousingViewModel viewModel = mapper.Map<HomeFinding, HousingViewModel>(order.HomeFinding);

            (viewModel.HousingTypeName).Should().Be(housingTypeName);
        }

        [TestMethod]
        public void TestDisplayForNumberOfBaths()
        {
            String bathTypeName = "2 1/2";
            NumberOfBathroomsType bathType = new NumberOfBathroomsType { Name = bathTypeName };
            order.HomeFinding.NumberOfBathrooms = bathType;

            HousingViewModel viewModel = mapper.Map<HomeFinding, HousingViewModel>(order.HomeFinding);

            (viewModel.NumberOfBathroomsName).Should().Be(bathTypeName);
        }
    }
}
