using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Systems
{
    public abstract class SystemBase
    {
        public abstract void Initialize();
        public abstract void Update();
        public abstract void Dispose();
    }
}
