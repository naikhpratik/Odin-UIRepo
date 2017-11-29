using Newtonsoft.Json;

namespace Odin.Domain.Bots.Dtos
{
    public class TruliaRentImageDto
    {
        [JsonProperty("standard_url")]
        public string Url { get; set; }
    }
}
