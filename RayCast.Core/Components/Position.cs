using RayCast.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Components
{
    public class Position : IComponent
    {
        //start positions
        public double PosX { get; set; }
        public double PosY { get; set; }

        //initial direcition
        public double DirX { get; set; }
        public double DirY { get; set; }

        public Position() { }

        public void SetPosition (double posX, double posY, double dirX, double dirY)
        {
            this.PosX = posX;
            this.PosY = posY;
            this.DirX = dirX;
            this.DirY = dirY;
        }
    }
}
