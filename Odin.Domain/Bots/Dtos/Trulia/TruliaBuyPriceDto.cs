using Newtonsoft.Json;

namespace Odin.Domain.Bots.Dtos
{
    public class TruliaBuyPriceDto
    {
        [JsonProperty("current")]
        public decimal? PropertyAmount { get; set; }
    }
}
