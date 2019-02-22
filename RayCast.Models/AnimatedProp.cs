using RayCast.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Models
{
    public class AnimatedProp : Prop
    {
        public Animation Animation { get; set; }

        public override void Update(params object[] arguments)
        {
            base.Update(arguments);
            Animation.Update();
            Sprite.Texture = Animation.TextureIds[Animation.CurrentFrame];
        }
    }
}
