using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Models
{
    public class Weapon
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsShooting { get; set; }

        private int[] _textureIds;
        private int _currentFrame;
        private int _frameCount;

        public Weapon(int width, int height, int[] texturesIds)
        {
            this.Width = width;
            this.Height = height;
            this.IsShooting = false;

            this._textureIds = texturesIds;
            this._frameCount = _textureIds.Length;
            this._currentFrame = 0;
        }

        public int GetCurrentFrame()
        {
            return _textureIds[_currentFrame];
        }

        public void UpdateNextFrame()
        {
            if (IsShooting && _currentFrame + 1 < _frameCount)
            {
                _currentFrame += 1;
            }
            else if (IsShooting && _currentFrame + 1 >= _frameCount)
            {
                _currentFrame = 0;
                IsShooting = false;
            }
        }

    }
}
