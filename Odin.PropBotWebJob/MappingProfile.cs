using AutoMapper;
using Odin.Data.Core.Models;
using Odin.PropBotWebJob.Dtos;
using Odin.PropBotWebJob.Dtos.Trulia;

namespace Odin.PropBotWebJob
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TruliaRentDto, Property>();
            CreateMap<TruliaBuyFeatureDto, Property>();
            CreateMap<TruliaBuyLocationDto, Property>();
            CreateMap<TruliaBuyPriceDto, Property>();
        }
    }
}
