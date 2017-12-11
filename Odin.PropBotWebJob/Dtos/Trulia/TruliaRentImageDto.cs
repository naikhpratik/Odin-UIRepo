using Newtonsoft.Json;

namespace Odin.PropBotWebJob.Dtos.Trulia
{
    public class TruliaRentImageDto
    {
        [JsonProperty("standard_url")]
        public string Url { get; set; }
    }
}
