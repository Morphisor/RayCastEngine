using RayCast.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RayCastEngine.NoConsole
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Engine window = new Engine(new System.Drawing.Size(740, 360)))
            {
                window.Title = "RayCast Engine";
                window.Run();
            }
        }
    }
}
