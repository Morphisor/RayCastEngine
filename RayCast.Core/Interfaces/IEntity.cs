using RayCast.Core.Components;
using RayCast.Core.Enums;
using RayCast.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Interfaces
{
    public interface IEntity
    {
        AnimatedSprite AnimatedSprite { get; set; }
        PathFinding PathFinding { get; set; }
        EntityStatus Status { get; set; }
        int[] AttackFrames { get; set; }
        int[] IdleFrames { get; set; }
        int[] DeathFrames { get; set; }
        int Ealth { get; set; }
        int Damage { get; set; }

        void UpdateEntity();
        void Attack();
        void TakeDamage();
    }
}
