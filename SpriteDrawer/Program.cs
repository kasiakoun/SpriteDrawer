using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SpriteDrawer
{
    class Program
    {
        static void Main(string[] args)
        {
            ISpriteSheetConverter spriteSheetConverter = new SpriteSheetConverter();
            //var test = string.Format("Some text {0}", args[0]);
            //var test2 = int.Parse(test);

            const string pathToImage = @"e:\Projects\mk3\src\assets\graphics\units\cyrax.png";
            const string pathToSprite = @"e:\Projects\mk3\src\assets\data\sprites\cyrax.json";
            var image = Image.FromFile(pathToImage);
            var sprite = spriteSheetConverter.LoadSpriteSheetByPath(pathToSprite);
            using (var graphiscs = Graphics.FromImage(image))
            {
                foreach (var animation in sprite.Animations)
                {
                    var pen = GenerateRandomPen();
                    foreach (var frame in animation.Frames)
                    {
                        var rectangle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
                        graphiscs.DrawRectangle(pen, rectangle);
                    }
                }

                image.Save("cyrax.bmp", ImageFormat.Bmp);
            }

            Console.WriteLine(Directory.GetCurrentDirectory());
            Console.ReadKey(true);
        }

        private static Pen GenerateRandomPen()
        {
            var ticks = (int)(DateTime.Now.Ticks % int.MaxValue);
            var rand = new Random(ticks);

            var color = Color.FromArgb(50, rand.Next(0, byte.MaxValue), rand.Next(0, byte.MaxValue), rand.Next(0, byte.MaxValue));
            var pen = new Pen(color);

            return pen;
        }
    }
}
