using RayCast.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Models
{
    public class OnCreateComponentArgs : EventArgs
    {
        public IComponent Component { get; set; }
        public int EntityId { get; set; }

        public OnCreateComponentArgs() { }
        public OnCreateComponentArgs(IComponent component, int entityId)
        {
            this.Component = component;
            this.EntityId = entityId;
        }
    }
}
