using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Primitives
{
    public class AnimatedSprite
    {
        public Sprite Sprite { get; set; }
        public int NumberOfFrames { get; }
        public int CurrentFrame { get; set; }
        public int[] Textures { get; }

        public AnimatedSprite(Sprite sprite, int[] textureIds)
        {
            this.Sprite = sprite;
            this.NumberOfFrames = textureIds.Length;
            this.CurrentFrame = 0;
            this.Textures = textureIds;
        }
    }
}
