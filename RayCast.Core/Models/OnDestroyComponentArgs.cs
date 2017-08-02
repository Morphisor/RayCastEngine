using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Models
{
    public class OnDestroyComponentArgs : EventArgs
    {
        public Type ComponentType { get; set; }
        public int EntityId { get; set; }

        public OnDestroyComponentArgs() { }
        public OnDestroyComponentArgs(Type componentType, int entityId)
        {
            this.ComponentType = componentType;
            this.EntityId = entityId;
        }
    }
}
