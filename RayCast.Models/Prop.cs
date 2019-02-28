using RayCast.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Models
{
    public class Prop : Actor
    {
        public Sprite Sprite { get; set; }

        public virtual void Update(params object[] arguments) { }
    }
}
