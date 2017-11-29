using Newtonsoft.Json;

namespace Odin.Domain.Bots.Dtos
{
    public class TruliaBuyFeatureDto
    {

        [JsonProperty("bedrooms")]
        public int? PropertyNumberOfBedrooms { get; set; }

        [JsonProperty("bathrooms")]
        public decimal? PropertyNumberOfBathrooms { get; set; }
    }
}
