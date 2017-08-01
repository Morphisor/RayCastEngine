﻿using RayCast.Core.Interfaces;
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

        public TComponent CreateComponent<TComponent>() where TComponent : IComponent
        {
            var component = _manager.CreateComponent<TComponent>(this.Id);

            //add new component to cache

            return component;
        }

        public bool DestroyComponent<TComponent>() where TComponent : IComponent
        {
            bool result = _manager.RemoveComponent<TComponent>(this.Id);
            return result;
        }

        public TComponent GetComponent<TComponent>() where TComponent : IComponent
        {
            var component = _manager.GetComponent<TComponent>(this.Id);
            return component;
        }

        public IEnumerable<IComponent> GetComponents()
        {
            var components = _manager.GetComponents(this.Id);
            return components;
        }
    }
}
