
using RayCast.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayCast.Models.BaseModels
{
    public class Movement
    {
        public MovingState MovingState { get; set; }
        public TurningState TurningState { get; set; }

        public Movement()
        {
            this.MovingState = MovingState.Idle;
            this.TurningState = TurningState.Idle;
        }
    }
}
