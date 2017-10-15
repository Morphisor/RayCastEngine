using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayCast.Core.Models;
using RayCast.Core.Enums;
using RayCast.Core.Components;

namespace RayCast.Core.Systems
{
    class HitDetectionSystem : SystemBase
    {
        private EntityManager _manager;
        private Entity _player;



        public HitDetectionSystem(EntityManager manager)
        {
            _manager = manager;
            _manager.OnCreateComponent += OnComponentCreated;
            _manager.OnDestroyComponent += OnComponentDestroyed;
        }


        public override void Initialize()
        {
            _player = _manager.EntititiesByType(EntityType.Player).First();
        }

        public override void Update()
        {
            
        }

        public void DamageEnemy(Entity enemyHit)
        {
            var playerWeapon = _player.GetComponent<PlayerWeapon>();
            var enemyStats = enemyHit.GetComponent<EnemyStats>();
            enemyStats.Life -= playerWeapon.Damage;

            if (enemyStats.Life <= 0)
            {

            }

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
