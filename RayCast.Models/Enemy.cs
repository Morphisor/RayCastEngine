using RayCast.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayCast.Models
{
    public class Enemy : AnimatedProp
    {
        private const double MOVEMENT_SPEED = 0.06;
        private const double ROTATION_SPEED = 0.15;

        public Enemy() { }

        public override void Update(params object[] arguments)
        {
            base.Update(arguments);

            if (!Sprite.IsVisible)
                return;

            Player player = arguments[0] as Player;
            int[,] worldMap = arguments[1] as int[,];

            var playerPosition = player.Position;
            int mapX = (int)playerPosition.PosX;
            int mapY = (int)playerPosition.PosY;

            if (!Sprite.IsVisible)
                return;

            int entityMapX = (int)Position.PosX;
            int entityMapY = (int)Position.PosY;

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

            int nextMapX = (int)((Position.PosX + 0.5) + dirX * MOVEMENT_SPEED);
            int nextMapY = (int)Position.PosY;

            if (worldMap[nextMapX, nextMapY] == 0 && nextMapX != mapX)
                Position.PosX += dirX * MOVEMENT_SPEED;
            else if (worldMap[nextMapX, nextMapY] != 0 && nextMapX != mapX)
                Position.PosY += dirY * MOVEMENT_SPEED;

            nextMapX = (int)Position.PosX;
            nextMapY = (int)((Position.PosY + 0.5) + dirY * MOVEMENT_SPEED);

            if (worldMap[nextMapX, nextMapY] == 0 && nextMapY != mapY)
                Position.PosY += dirY * MOVEMENT_SPEED;
            else if (worldMap[nextMapX, nextMapY] != 0 && nextMapY != mapY)
                Position.PosX += dirX * MOVEMENT_SPEED;
        }
    }
}
