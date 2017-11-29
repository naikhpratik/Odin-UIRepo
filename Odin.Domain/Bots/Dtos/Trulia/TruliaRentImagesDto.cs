using Newtonsoft.Json;
using System.Collections.Generic;

namespace Odin.Domain.Bots.Dtos
{
    public class TruliaRentImagesDto
    {
        [JsonProperty("photos")]
        public List<TruliaRentImageDto> Images { get; set; }
    }
}
