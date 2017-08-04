using RayCast.Core.Components;
using RayCast.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Systems
{
    public class AnimationSystem : SystemBase
    {

        private EntityManager _manager;
        private Dictionary<int, Animation> _animatedSprites;
        private Dictionary<int, SpriteComponent> _relatedSprites;

        public AnimationSystem(EntityManager manager)
        {
            _manager = manager;
            _manager.OnCreateComponent += OnComponentCreated;
            _manager.OnDestroyComponent += OnComponentDestroyed;
        }

        public override void Dispose()
        {
            _manager.OnCreateComponent -= OnComponentCreated;
            _manager.OnDestroyComponent -= OnComponentDestroyed;

            foreach (int key in _animatedSprites.Keys)
                _manager.DestroyEntity(key);
        }

        public override void Initialize()
        {
            var animationComponents = _manager.GetAllComponents<Animation>();
            var spriteComponents = _manager.GetAllComponents<SpriteComponent>();

            var missingAnimation = spriteComponents.Keys.Except(animationComponents.Keys).ToList();
            missingAnimation.ForEach(x => spriteComponents.Remove(x));

            _animatedSprites = animationComponents.ToDictionary(k => k.Key, v => v.Value as Animation);
            _relatedSprites = spriteComponents.ToDictionary(k => k.Key, v => v.Value as SpriteComponent);
        }

        public override void Update()
        {
            foreach (int key in _animatedSprites.Keys)
            {
                Animation animation = _animatedSprites[key];
                SpriteComponent sprite = _relatedSprites[key];

                animation.CurrentFrame += 1;
                if (animation.CurrentFrame > animation.FrameCount - 1)
                    animation.CurrentFrame = 0;

                sprite.Texture = animation.TextureIds[animation.CurrentFrame];
            }
        }

        public override void OnComponentCreated(OnCreateComponentArgs e)
        {
            var animationToAdd = _manager.GetComponent<Animation>(e.EntityId);
            var spriteToAdd = _manager.GetComponent<SpriteComponent>(e.EntityId);

            if (e.Component is Animation && spriteToAdd != null)
                animationToAdd = e.Component as Animation;

            else if (e.Component is SpriteComponent && animationToAdd != null)
                spriteToAdd = e.Component as SpriteComponent;

            if (_relatedSprites.ContainsKey(e.EntityId))
                _relatedSprites[e.EntityId] = spriteToAdd;
            else
                _relatedSprites.Add(e.EntityId, spriteToAdd);

            if (_animatedSprites.ContainsKey(e.EntityId))
                _animatedSprites[e.EntityId] = animationToAdd;
            else
                _animatedSprites.Add(e.EntityId, animationToAdd);
        }

        public override void OnComponentDestroyed(OnDestroyComponentArgs e)
        {
            if (e.ComponentType == typeof(SpriteComponent) || e.ComponentType == typeof(Animation))
            {
                _relatedSprites.Remove(e.EntityId);
                _animatedSprites.Remove(e.EntityId);
            }
        }
    }
}
