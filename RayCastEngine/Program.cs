using RayCast.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCastEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Engine window = new Engine(new System.Drawing.Size(740, 360)))
            {
                window.Title = "RayCast Engine";
                window.Run();
            }
        }
    }
}