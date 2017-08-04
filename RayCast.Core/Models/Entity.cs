using RayCast.Core.Enums;
using RayCast.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Models
{
    public class Entity : IEntity
    {
        //cache most used components

        private readonly EntityManager _manager;

        public int Id { get; }
        public EntityType EntityType { get; set; }

        public Entity(EntityManager manager, int entityId, EntityType type)
        {
            _manager = manager;
            Id = entityId;
            EntityType = type;
        }

        public TComponent CreateComponent<TComponent>() 
            where TComponent : IComponent, new()
        {
            TComponent component = _manager.CreateComponent<TComponent>(this.Id);

            //add to cache if is a cache item

            return component;
        }

        public bool DestroyComponent<TComponent>() 
            where TComponent : IComponent
        {
            bool result = _manager.RemoveComponent<TComponent>(this.Id);
            
            //remove from cache if is a cache item

            return result;
        }

        public TComponent GetComponent<TComponent>() 
            where TComponent : class, IComponent
        {
            TComponent component = _manager.GetComponent<TComponent>(this.Id);
            return component;
        }

        public IEnumerable<IComponent> GetComponents()
        {
            IEnumerable<IComponent> copmonents = _manager.GetComponents(this.Id);
            return copmonents;
        }
    }
}
