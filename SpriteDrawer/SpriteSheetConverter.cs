using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpriteDrawer
{
    public class SpriteSheetConverter : ISpriteSheetConverter
    {
        #region ISpriteSheetConverter Implemenetation

        public SpriteSheet LoadSpriteSheetByPath(string path)
        {
            var jsonText = File.ReadAllText(path);
            var spriteSheet = JsonConvert.DeserializeObject<SpriteSheet>(jsonText);
            return spriteSheet;
        }

        public void SaveSpriteSheetByPath(SpriteSheet spriteSheet, string path)
        {
            var jsonText = JsonConvert.SerializeObject(spriteSheet, Formatting.Indented);
            File.WriteAllText(path, jsonText);
        }

        #endregion
    }
}
