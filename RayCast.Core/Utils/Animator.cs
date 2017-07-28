using RayCast.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Utils
{
    public class Animator
    {

        private List<AnimatedSprite> _animatedSprites;
        private Textures _textures;

        public Animator(Textures textures)
        {
            _animatedSprites = new List<AnimatedSprite>();
            _textures = textures;
        }

        public void AddAnimatedSprite(AnimatedSprite sprite)
        {
            sprite.Sprite.Texture = sprite.Textures[0];
            _animatedSprites.Add(sprite);
        }

        public void AddAnimatedSprites(AnimatedSprite[] sprites)
        {
            for (int i = 0; i < sprites.Length; i++)
                sprites[i].Sprite.Texture = sprites[i].Textures[0];

            _animatedSprites.AddRange(sprites);
        }

        public void UpdateCurrentFrame()
        {
            for (int i = 0; i < _animatedSprites.Count; i++)
            {
                Sprite sprite = _animatedSprites[i].Sprite;

                int nextFrame = _animatedSprites[i].CurrentFrame + 1;
                if (nextFrame > _animatedSprites[i].NumberOfFrames - 1)
                    nextFrame = 0;
                
                _animatedSprites[i].CurrentFrame = nextFrame;
                sprite.Texture = _animatedSprites[i].Textures[nextFrame];
                
            }
        }
    }
}
