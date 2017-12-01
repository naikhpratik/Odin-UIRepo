using Newtonsoft.Json;

namespace Odin.PropBotWebJob.Dtos.Trulia
{
    public class TruliaBuyPriceDto
    {
        [JsonProperty("current")]
        public decimal? Amount { get; set; }
    }
}
