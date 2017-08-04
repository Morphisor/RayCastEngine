using RayCast.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Components
{
    public class Camera : IComponent
    {
        public double PlaneX { get; set; }
        public double PlaneY { get; set; }

        public Camera() { }

        public void SetCameraPlane(double planeX, double planeY)
        {
            this.PlaneX = planeX;
            this.PlaneY = planeY;
        }
    }
}
