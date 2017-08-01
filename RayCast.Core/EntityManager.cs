using RayCast.Core.Interfaces;
using RayCast.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core
{
    public class EntityManager
    {
        Entity this[int index] => _entities[index];

        private readonly Dictionary<int, Entity> _entities;
        private readonly Dictionary<Type, Dictionary<int, IComponent>> _components;

        public Entity CreateEntity()
        {
            throw new NotImplementedException();
        }

        public bool DestroyEntity(int id)
        {
            throw new NotImplementedException();
        }

        public TComponent CreateComponent<TComponent>(int entityId)
            where TComponent : IComponent
        {
            throw new NotImplementedException();
        }

        public bool RemoveComponent<TComponent>(int entityId)
            where TComponent : IComponent
        {
            throw new NotImplementedException();
        }

        public TComponent GetComponent<TComponent>(int entityId)
            where TComponent : IComponent
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IComponent> GetComponents(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
