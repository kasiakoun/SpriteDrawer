using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpriteDrawer
{
    public class SpriteSheet
    {
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("animations")]
        public List<Animation> Animations { get; set; }
    }
}
