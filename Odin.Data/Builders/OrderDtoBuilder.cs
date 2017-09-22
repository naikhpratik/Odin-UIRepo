using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Odin.Data.Core.Dtos;
using Odin.Data.Helpers;

namespace Odin.Data.Builders
{
    public static class OrderDtoBuilder
    {
        public static OrderDto New()
        {
            var relocationTypes = new[] { "International", "Domestic" };

            var orderFaker = new Faker<OrderDto>()
                .RuleFor(o => o.RelocationType, f => f.PickRandom(relocationTypes))
                .RuleFor(o => o.DestinationCity, f => f.Address.City())
                .RuleFor(o => o.DestinationState, f => f.Address.State())
                .RuleFor(o => o.DestinationCountry, f => f.Address.CountryCode())
                .RuleFor(o => o.DestinationZip, f => f.Address.ZipCode())
                .RuleFor(o => o.OriginCity, f => f.Address.City())
                .RuleFor(o => o.OriginCountry, f => f.Address.CountryCode())
                .RuleFor(o => o.OriginState, f => f.Address.State())
                .RuleFor(o => o.EstimatedArrivalDate, f => f.Date.Future())
                .RuleFor(o => o.TrackingId, f=> $"integration-{f.IndexGlobal}");

            var orderDto = orderFaker.Generate();

            orderDto.TrackingId = TokenHelper.NewToken();

            return orderDto;
        }
    }
}
