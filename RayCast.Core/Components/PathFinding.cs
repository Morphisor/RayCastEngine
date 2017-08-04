using RayCast.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Components
{
    public class PathFinding
    {

        private const double MOVEMENT_SPEED = 0.06;
        private const double ROTATION_SPEED = 0.15;

        private List<SpriteComponent> _entities;
        private int[,] _worldMap;

        public PathFinding(int[,] worldMap)
        {
            _worldMap = worldMap;
            _entities = new List<SpriteComponent>();
        }

        public void AddEntity(SpriteComponent entity)
        {
            _entities.Add(entity);
        }

        public void UpdateEntityPosition(double pointX, double pointY)
        {
            int mapX = (int)pointX;
            int mapY = (int)pointY;

            for (int i = 0; i < _entities.Count; i++)
            {
                if (!_entities[i].IsVisible)
                    continue;

                int entityMapX = (int)_entities[i].X;
                int entityMapY = (int)_entities[i].Y;

                int distanceX = (entityMapX - mapX) + 1;
                int distanceY = (entityMapY - mapY) + 1;

                int dirX = 0;
                int dirY = 0;

                if (distanceX > 0)
                    dirX = -1;
                else if (distanceX < 0)
                    dirX = 1;

                if (distanceY > 0)
                    dirY = -1;
                else if (distanceY < 0)
                    dirY = 1;

                int nextMapX = (int)((_entities[i].X + 0.5) + dirX * MOVEMENT_SPEED);
                int nextMapY = (int)_entities[i].Y;

                if (_worldMap[nextMapX, nextMapY] == 0 && nextMapX != mapX)
                    _entities[i].X += dirX * MOVEMENT_SPEED;
                else if (_worldMap[nextMapX, nextMapY] != 0 && nextMapX != mapX)
                    _entities[i].Y += dirY * MOVEMENT_SPEED;

                nextMapX = (int)_entities[i].X;
                nextMapY = (int)((_entities[i].Y + 0.5) + dirY * MOVEMENT_SPEED);

                if (_worldMap[nextMapX, nextMapY] == 0 && nextMapY != mapY)
                    _entities[i].Y += dirY * MOVEMENT_SPEED;
                else if (_worldMap[nextMapX, nextMapY] != 0 && nextMapY != mapY)
                    _entities[i].X += dirX * MOVEMENT_SPEED;
            }
        }
    }
}
