using System;
using System.Collections.Generic;
using System.Text;

namespace RayCast.Models.BaseModels
{
    public class Weapon
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public bool IsShooting { get; set; }
        public int Damage { get; set; }

        public int HitMarginLeft { get; set; }
        public int HitMarginRight { get; set; }

        private int[] _textureIds;
        private int _currentFrame;
        private int _frameCount;

        public Weapon(int width, int height, int[] texturesIds, int viewPortX)
        {
            this.Width = width;
            this.Height = height;

            this.HitMarginLeft = (viewPortX / 2) - 10;
            this.HitMarginRight = (viewPortX / 2) + 10;

            this.IsShooting = false;
            this.Damage = 50;

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
