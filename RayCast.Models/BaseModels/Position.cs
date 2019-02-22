using System;
using System.Collections.Generic;
using System.Text;

namespace RayCast.Models.BaseModels
{
    public class Position
    {
        //start positions
        public double PosX { get; set; }
        public double PosY { get; set; }

        //initial direcition
        public double DirX { get; set; }
        public double DirY { get; set; }

        public Position(double posX, double posY, double dirX, double dirY)
        {
            this.PosX = posX;
            this.PosY = posY;
            this.DirX = dirX;
            this.DirY = dirY;
        }
    }
}
