using Newtonsoft.Json;

namespace Odin.PropBotWebJob.Dtos.Trulia
{
    public class TruliaBuyFeatureDto
    {

        [JsonProperty("bedrooms")]
        public int? NumberOfBedrooms { get; set; }

        [JsonProperty("bathrooms")]
        public decimal? NumberOfBathrooms { get; set; }
    }
}
