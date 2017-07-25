using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Primitives
{
    public class Textures
    {
        public Dictionary<int, Pixel[]> TextureBuffer { get; set; }
        public int TexturesWidth { get;}
        public int TexturesHeight { get;}

        public Pixel[] this[int index]
        {
            get
            {
                return TextureBuffer[index];
            }
        }

        public Textures(int texturesWidth, int texturesHeight)
        {
            TextureBuffer = new Dictionary<int, Pixel[]>();
            TexturesWidth = texturesWidth;
            TexturesHeight = texturesHeight;
        }

        public void Add(string fileName, int index, int width = 64, int height = 64)
        {
            Bitmap textureBmp = new Bitmap($"Resources\\textures\\{fileName}");
            Pixel[] texture = new Pixel[width * height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    texture[x * width + y] = new Pixel();
                    texture[x * width + y].Color = textureBmp.GetPixel(y, x);
                    texture[x * width + y].X = x;
                    texture[x * width + y].Y = y;
                }
            }
            TextureBuffer.Add(index, texture);
        }
    }
}
