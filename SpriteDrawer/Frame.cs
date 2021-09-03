using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpriteDrawer
{
    public class Frame
    {
        [JsonProperty("x")]
        public int X { get; set; }
        [JsonProperty("y")]
        public int Y { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("offsetX")]
        public int OffsetX { get; set; }
        [JsonProperty("offsetY")]
        public int OffsetY { get; set; }
        [JsonProperty("duplicates")]
        public int Duplicates { get; set; }
    }
}
