using RayCast.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Models.BaseModels
{
    public class Animation
    {
        public int[] TextureIds { get; set; }
        public int CurrentFrame { get; set; }
        public int FrameCount { get; set; }
        public AnimationType Type { get; set; }

        public Animation(int[] textureIds, AnimationType type)
        {
            this.TextureIds = textureIds;
            this.CurrentFrame = 0;
            this.FrameCount = textureIds.Length;
            this.Type = type;
        }

        public void Update()
        {
            CurrentFrame += 1;
            if (CurrentFrame > FrameCount - 1)
                CurrentFrame = 0;
        }
    }
}
