using Newtonsoft.Json;
using System.Collections.Generic;

namespace Odin.PropBotWebJob.Dtos.Trulia
{
    public class TruliaBuyImagesDto
    {
        [JsonProperty("mediaCollection")]
        public List<TruliaBuyImageDto> Images { get; set; }
    }
}
