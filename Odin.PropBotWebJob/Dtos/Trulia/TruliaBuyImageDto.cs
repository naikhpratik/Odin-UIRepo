using Newtonsoft.Json;

namespace Odin.PropBotWebJob.Dtos.Trulia
{
    public class TruliaBuyImageDto
    {
        [JsonProperty("original")]
        public string Url { get; set; }
    }
}
