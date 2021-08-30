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
        public Animation[] Animations { get; set; }
    }
}
