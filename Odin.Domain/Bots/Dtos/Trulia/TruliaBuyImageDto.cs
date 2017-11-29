using Newtonsoft.Json;

namespace Odin.Domain.Bots.Dtos
{
    public class TruliaBuyImageDto
    {
        [JsonProperty("original")]
        public string Url { get; set; }
    }
}
