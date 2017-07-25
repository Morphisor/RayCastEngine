using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Primitives
{
    public class Sprite
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int Texture { get; set; }

        public Sprite(double x, double y, int texture)
        {
            this.X = x;
            this.Y = y;
            this.Texture = texture;
        }
    }
}
