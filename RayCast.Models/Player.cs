using RayCast.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayCast.Models
{
    public class Player : Actor
    {
        public Camera Camera { get; set; }
        public Movement Movement { get; set; }
        public Weapon Weapon { get; set; }

        public Player()
        {

        }
    }
}
