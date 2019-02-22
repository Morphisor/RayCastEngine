using System;
using System.Collections.Generic;
using System.Text;

namespace RayCast.Models.BaseModels
{
    public class Camera
    {
        public double PlaneX { get; set; }
        public double PlaneY { get; set; }

        public Camera(double planeX, double planeY)
        {
            this.PlaneX = planeX;
            this.PlaneY = planeY;
        }

    }
}
