using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayCast.Core.Models;

namespace RayCast.Core.Systems
{
    public class StateMachineSystem : SystemBase
    {

        private EntityManager _manager;

        public StateMachineSystem(EntityManager manager)
        {
            _manager = manager;
            _manager.OnCreateComponent += OnComponentCreated;
            _manager.OnDestroyComponent += OnComponentDestroyed;
        }

        public override void Initialize()
        {

        }

        public override void Update()
        {

        }

        public override void OnComponentCreated(OnCreateComponentArgs e)
        {

        }

        public override void OnComponentDestroyed(OnDestroyComponentArgs e)
        {

        }

        public override void Dispose()
        {
            _manager.OnCreateComponent -= OnComponentCreated;
            _manager.OnDestroyComponent -= OnComponentDestroyed;
        }
    }
}
