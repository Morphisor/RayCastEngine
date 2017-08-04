using RayCast.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Components
{
    public class SpriteComponent : IComponent
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int Texture { get; set; }
        public bool IsVisible { get; set; }

        public SpriteComponent() { }

        public void InitSprite(double x, double y, int texture)
        {
            this.X = x;
            this.Y = y;
            this.Texture = texture;
            this.IsVisible = false;
        }
    }
}
