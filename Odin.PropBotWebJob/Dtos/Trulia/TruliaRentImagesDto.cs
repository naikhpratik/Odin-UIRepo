using Newtonsoft.Json;
using System.Collections.Generic;

namespace Odin.PropBotWebJob.Dtos.Trulia
{
    public class TruliaRentImagesDto
    {
        [JsonProperty("photos")]
        public List<TruliaRentImageDto> Images { get; set; }
    }
}
