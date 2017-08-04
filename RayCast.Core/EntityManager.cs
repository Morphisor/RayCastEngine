using RayCast.Core.Enums;
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

        public event OnCreateComponentHandler OnCreateComponent;
        public event OnDestroyComponentHandler OnDestroyComponent;

        public delegate void OnCreateComponentHandler(OnCreateComponentArgs e);
        public delegate void OnDestroyComponentHandler(OnDestroyComponentArgs e);

        private readonly Dictionary<int, Entity> _entities;
        private readonly Dictionary<Type, Dictionary<int, IComponent>> _components;
        private int _nextId;

        public EntityManager()
        {
            _entities = new Dictionary<int, Entity>();
            _components = new Dictionary<Type, Dictionary<int, IComponent>>();
            _nextId = 0;
        }

        public Entity CreateEntity()
        {
            Entity entity = new Entity(this, _nextId);
            _entities.Add(_nextId, entity);
            _nextId++;

            return entity;
        }

        public bool DestroyEntity(int id)
        {
            for (int i = 0; i < _components.Keys.Count; i++)
            {
                Type key = _components.Keys.ElementAt(i);
                _components[key].Remove(id);
            }

            if (_entities.Remove(id))
            {
                _nextId--;
                return true;
            }

            return false;
        }

        public TComponent CreateComponent<TComponent>(int entityId)
            where TComponent : IComponent, new()
        {
            TComponent component = new TComponent();
            Type componentType = typeof(TComponent);

            if (_components.ContainsKey(componentType))
            {
                _components[componentType].Add(entityId, component);
            }
            else
            {
                _components.Add(componentType, new Dictionary<int, IComponent>());
                _components[componentType].Add(entityId, component);
            }

            OnCreateComponent?.Invoke(new OnCreateComponentArgs(component, entityId));

            return component;
        }

        public bool RemoveComponent<TComponent>(int entityId)
            where TComponent : IComponent
        {
            Type componentType = typeof(TComponent);

            if (_components[componentType].Remove(entityId))
            {
                OnDestroyComponent?.Invoke(new OnDestroyComponentArgs(componentType, entityId));
                return true;
            }

            return false;
        }

        public TComponent GetComponent<TComponent>(int entityId)
            where TComponent : class, IComponent
        {
            Type componentType = typeof(TComponent);

            if (_components[componentType].ContainsKey(entityId))
                return _components[componentType][entityId] as TComponent;
            else
                return null;
        }

        public IEnumerable<IComponent> GetComponents(int entityId)
        {
            List<IComponent> components = new List<IComponent>();
            for (int i = 0; i < _components.Keys.Count; i++)
            {
                Type key = _components.Keys.ElementAt(i);
                if (_components[key].ContainsKey(entityId))
                    components.Add(_components[key][entityId]);
            }
            return components;
        }

        public Dictionary<int, IComponent> GetAllComponents<TComponent>()
            where TComponent : class, IComponent
        {
            Dictionary<int, IComponent> components = new Dictionary<int, IComponent>();
            foreach (int entityId in _entities.Keys)
            {
                IComponent component = _entities[entityId].GetComponent<TComponent>();
                if (component != null)
                    components.Add(entityId, component);
            }

            return components;
        }

        public IEnumerable<Entity> EntititiesByType(EntityType entityType)
        {
            return _entities.Where(x => x.Value.EntityType == EntityType.Enemy).Select(x => x.Value);
        }
    }
}
