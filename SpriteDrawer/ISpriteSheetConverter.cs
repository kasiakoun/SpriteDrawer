using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteDrawer
{
    public interface ISpriteSheetConverter
    {
        SpriteSheet LoadSpriteSheetByPath(string path);
        void SaveSpriteSheetByPath(SpriteSheet spriteSheet, string path);
    }
}
