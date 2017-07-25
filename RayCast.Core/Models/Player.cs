using RayCast.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Models
{
    public class Player
    {
        //start positions
        public double PosX { get; set; }
        public double PosY { get; set; }

        //initial direcition
        public double DirX { get; set; }
        public double DirY { get; set; }

        public TurningState TurningState { get; set; }
        public MovingState MovingState { get; set; }
        
        public Player() { }
        public Player(double posX, double posY, double dirX, double dirY)
        {
            this.PosX = posX;
            this.PosY = posY;
            this.DirX = dirX;
            this.DirY = dirY;

            this.MovingState = MovingState.Idle;
            this.TurningState = TurningState.Idle;
        }
    }
}
