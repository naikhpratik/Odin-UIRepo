using Newtonsoft.Json;
using System.Collections.Generic;

namespace Odin.Domain.Bots.Dtos
{
    public class TruliaBuyImagesDto
    {
        [JsonProperty("mediaCollection")]
        public List<TruliaBuyImageDto> Images { get; set; }
    }
}
