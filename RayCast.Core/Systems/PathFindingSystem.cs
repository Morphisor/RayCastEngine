using RayCast.Core.Components;
using RayCast.Core.Enums;
using RayCast.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCast.Core.Systems
{
    public class PathFindingSystem : SystemBase
    {
        private EntityManager _manager;
        private Dictionary<int, SpriteComponent> _relatedSprites;
        private int[,] _worldMap;

        private const double MOVEMENT_SPEED = 0.06;
        private const double ROTATION_SPEED = 0.15;

        public PathFindingSystem(EntityManager manager, int[,] worldMap)
        {
            _manager = manager;
            _worldMap = worldMap;
        }

        public override void Dispose()
        {
            foreach (int key in _relatedSprites.Keys)
            {
                _manager.DestroyEntity(key);
            }
        }

        public override void Initialize()
        {
            List<Entity> enemies = _manager.EntititiesByType(EntityType.Enemy).ToList();
            foreach (Entity enemy in enemies)
            {
                var sprite = enemy.GetComponent<SpriteComponent>();

                if (sprite != null)
                    _relatedSprites.Add(enemy.Id, sprite);
            }
        }

        public override void Update()
        {
            Entity player = _manager.EntititiesByType(EntityType.Player).First();
            Position playerPostion = player.GetComponent<Position>();

            int mapX = (int)playerPostion.PosX;
            int mapY = (int)playerPostion.PosY;

            foreach(int key in _relatedSprites.Keys)
            {

                SpriteComponent sprite = _relatedSprites[key];

                if (!sprite.IsVisible)
                    continue;

                int entityMapX = (int)sprite.X;
                int entityMapY = (int)sprite.Y;

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

                int nextMapX = (int)((sprite.X + 0.5) + dirX * MOVEMENT_SPEED);
                int nextMapY = (int)sprite.Y;

                if (_worldMap[nextMapX, nextMapY] == 0 && nextMapX != mapX)
                    sprite.X += dirX * MOVEMENT_SPEED;
                else if (_worldMap[nextMapX, nextMapY] != 0 && nextMapX != mapX)
                    sprite.Y += dirY * MOVEMENT_SPEED;

                nextMapX = (int)sprite.X;
                nextMapY = (int)((sprite.Y + 0.5) + dirY * MOVEMENT_SPEED);

                if (_worldMap[nextMapX, nextMapY] == 0 && nextMapY != mapY)
                    sprite.Y += dirY * MOVEMENT_SPEED;
                else if (_worldMap[nextMapX, nextMapY] != 0 && nextMapY != mapY)
                    sprite.X += dirX * MOVEMENT_SPEED;
            }
        }
    }
}
