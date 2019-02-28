using RayCast.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Models
{
    public class StatefulProp<T> : Prop where T : struct
    {
        public Animation CurrentAnimation { get; set; }
        public T State { get; set; }
        public List<Animation> Animations { get; set; }

        public override void Update(params object[] arguments)
        {
            base.Update(arguments);

            if (Sprite.IsVisible)
            {
                CurrentAnimation.Update();
                Sprite.Texture = CurrentAnimation.TextureIds[CurrentAnimation.CurrentFrame];
            }

        }
    }
}
