using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Primitives
{
    public class Sprite
    {
        public int Texture { get; set; }
        public bool IsVisible { get; set; }

        public Sprite() { }

        public Sprite(int texture)
        {
            this.Texture = texture;
            this.IsVisible = false;
        }
    }
}
