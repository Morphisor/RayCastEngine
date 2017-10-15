using RayCast.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Components
{
    public class EnemyStats : IComponent
    {
        public int Life { get; set; }
        public int Damage { get; set; }

        public EnemyStats()
        {
            this.Life = 100;
            this.Damage = 50;
        }


    }
}
