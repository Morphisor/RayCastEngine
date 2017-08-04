using RayCast.Core.Components;
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
        }

        public override void Dispose()
        {
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
    }
}
