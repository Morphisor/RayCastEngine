using RayCast.Core.Enums;
using RayCast.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Components
{
    public class Movement : IComponent
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
