using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Primitives
{
    public class Ray
    {
        //ray position and direction
        public double RayPosX { get; set; }
        public double RayPosY { get; set; }
        public double RayDirX { get; set; }
        public double RayDirY { get; set; }

        //legth of ray
        public double DeltaDistX { get; set; }
        public double DeltaDistY { get; set; }

        //side of wall hit
        public int Side { get; set; }

        //position in the map
        public int MapX { get; set; }
        public int MapY { get; set; }

        public Ray(double rayPosX, double rayPosY, double rayDirX, double rayDirY)
        {
            this.RayPosX = rayPosX;
            this.RayPosY = rayPosY;
            this.RayDirX = rayDirX;
            this.RayDirY = rayDirY;

            this.DeltaDistX = Math.Sqrt(1 + (rayDirY * rayDirY) / (rayDirX * rayDirX));
            this.DeltaDistY = Math.Sqrt(1 + (rayDirX * rayDirX) / (rayDirY * rayDirY));

            this.Side = 0;

            this.MapX = (int)rayPosX;
            this.MapY = (int)rayPosY;
        }
    }
}
