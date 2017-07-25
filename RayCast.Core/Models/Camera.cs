using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Models
{
    public class Camera
    {
        //camera plane
        public double PlaneX { get; set; }
        public double PlaneY { get; set; }

        public Camera(double planeX, double planeY)
        {
            this.PlaneX = planeX;
            this.PlaneY = planeY;
        }
    }
}
