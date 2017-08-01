using RayCast.Core.Components;
using RayCast.Core.Enums;
using RayCast.Core.Models;
using RayCast.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Interfaces
{
    public interface IEntity
    {
        int Id { get; }

        TComponent CreateComponent<TComponent>()
            where TComponent : IComponent, new();

        bool DestroyComponent<TComponent>()
            where TComponent : IComponent;

        TComponent GetComponent<TComponent>()
            where TComponent : class, IComponent;

        IEnumerable<IComponent> GetComponents();
    }
}
