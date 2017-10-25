using RayCast.Core.Enums;
using RayCast.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Components
{
    public class Animation : IComponent
    {
        public int[] TextureIds { get; set; }
        public int CurrentFrame { get; set; }
        public int FrameCount { get; set; }
        public AnimationType Type { get; set; }

        public Animation() { }

        public void InitAnimation(int[] textureIds, AnimationType type)
        {
            this.TextureIds = textureIds;
            this.CurrentFrame = 0;
            this.FrameCount = textureIds.Length;
            this.Type = type;
        }
    }
}
