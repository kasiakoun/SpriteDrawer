using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpriteDrawer
{
    public class Animation
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("repeatAnimation")]
        public bool RepeatAnimation { get; set; }
        [JsonProperty("frames")]
        public Frame[] Frames { get; set; }
    }
}
