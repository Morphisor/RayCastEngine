﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using RayCast.Core.Utils;
using System.Windows.Forms;
using RayCast.Core.Primitives;
using RayCast.Core.Enums;
using System.Runtime;
using RayCast.Core.Models;
using System.Diagnostics;

namespace RayCast.Core
{
    public class Engine : GameWindow
    {

        private static double TARGET_UPDATES_PER_SECOND = 30;
        private static double MOVEMENT_SPEED = 0.080;
        private static double ROT_SPEED = 0.15;
        private static int NUM_SPRITES = 19;

        private int[,] _worldMap;

        private readonly Size _windowSize;
        private double _timeFromLastUpdate;
        private double _fps;
        private long _renderingCalculation;

        private Textures _textures;
        private Sprite[] _sprites;

        private Pixel[] _drawingBuffer;
        private double[] _zBuffer;

        private Player _player;
        private Camera _camera;

        private Dictionary<int, double> _distLookUp;

        public Engine(Size windowSize) : base(windowSize.Width, windowSize.Height)
        {
            _windowSize = windowSize;
        }

        protected override void OnLoad(EventArgs e)
        {
            _drawingBuffer = new Pixel[_windowSize.Width * _windowSize.Height + 1];
            _zBuffer = new double[_windowSize.Width];

            //map setup
            _worldMap = new int[,]
            {
                {8,8,8,8,8,8,8,8,8,8,8,4,4,6,4,4,6,4,6,4,4,4,6,4},
                {8,0,0,0,0,0,0,0,0,0,8,4,0,0,0,0,0,0,0,0,0,0,0,4},
                {8,0,3,3,0,0,0,0,0,8,8,4,0,0,0,0,0,0,0,0,0,0,0,6},
                {8,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6},
                {8,0,3,3,0,0,0,0,0,8,8,4,0,0,0,0,0,0,0,0,0,0,0,4},
                {8,0,0,0,0,0,0,0,0,0,8,4,0,0,0,0,0,6,6,6,0,6,4,6},
                {8,8,8,8,0,8,8,8,8,8,8,4,4,4,4,4,4,6,0,0,0,0,0,6},
                {7,7,7,7,0,7,7,7,7,0,8,0,8,0,8,0,8,4,0,4,0,6,0,6},
                {7,7,0,0,0,0,0,0,7,8,0,8,0,8,0,8,8,6,0,0,0,0,0,6},
                {7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,6,0,0,0,0,0,4},
                {7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,8,6,0,6,0,6,0,6},
                {7,7,0,0,0,0,0,0,7,8,0,8,0,8,0,8,8,6,4,6,0,6,6,6},
                {7,7,7,7,0,7,7,7,7,8,8,4,0,6,8,4,8,3,3,3,0,3,3,3},
                {2,2,2,2,0,2,2,2,2,4,6,4,0,0,6,0,6,3,0,0,0,0,0,3},
                {2,2,0,0,0,0,0,2,2,4,0,0,0,0,0,0,4,3,0,0,0,0,0,3},
                {2,0,0,0,0,0,0,0,2,4,0,0,0,0,0,0,4,3,0,0,0,0,0,3},
                {1,0,0,0,0,0,0,0,1,4,4,4,4,4,6,0,6,3,3,0,0,0,3,3},
                {2,0,0,0,0,0,0,0,2,2,2,1,2,2,2,6,6,0,0,5,0,5,0,5},
                {2,2,0,0,0,0,0,2,2,2,0,0,0,2,2,0,5,0,5,0,0,0,5,5},
                {2,0,0,0,0,0,0,0,2,0,0,0,0,0,2,5,0,5,0,5,0,5,0,5},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5},
                {2,0,0,0,0,0,0,0,2,0,0,0,0,0,2,5,0,5,0,5,0,5,0,5},
                {2,2,0,0,0,0,0,2,2,2,0,0,0,2,2,0,5,0,5,0,0,0,5,5},
                {2,2,2,2,1,2,2,2,2,2,2,1,2,2,2,5,5,5,5,5,5,5,5,5}
            };

            _sprites = new Sprite[]
            {
                  new Sprite(20.5, 11.5, 10),
                  new Sprite(18.5,4.5, 10),
                  new Sprite(10.0,4.5, 10),
                  new Sprite(10.0,12.5,10),
                  new Sprite(3.5, 6.5, 10),
                  new Sprite(3.5, 20.5,10),
                  new Sprite(3.5, 14.5,10),
                  new Sprite(14.5,20.5,10),
                  new Sprite(18.5, 10.5, 9),
                  new Sprite(18.5, 11.5, 9),
                  new Sprite(18.5, 12.5, 9),
                  new Sprite(21.5, 1.5, 8),
                  new Sprite(15.5, 1.5, 8),
                  new Sprite(16.0, 1.8, 8),
                  new Sprite(16.2, 1.2, 8),
                  new Sprite(3.5,  2.5, 8),
                  new Sprite(9.5, 15.5, 8),
                  new Sprite(10.0, 15.1,8),
                  new Sprite(10.5, 15.8,8),
            };

            _player = new Player(22, 12, -1, 0);
            _camera = new Camera(0, 0.66);

            //setup input
            Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(KeyboardKeyDown);
            Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(KeyboardKeyUp);

            //setup drawing mode
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, _windowSize.Width, _windowSize.Height, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Modelview);

            //setup textures
            _textures = new Textures(64, 64);

            _textures.Add("redbrick.png", 0);
            _textures.Add("wood.png", 1);
            _textures.Add("mossy.png", 2);
            _textures.Add("purplestone.png", 3);
            _textures.Add("eagle.png", 4);
            _textures.Add("colorstone.png", 5);
            _textures.Add("greystone.png", 6);
            _textures.Add("bluestone.png", 7);

            _textures.Add("barrel.png", 8);
            _textures.Add("pillar.png", 9);
            _textures.Add("greenlight.png", 10);

            //init lookup
            _distLookUp = new Dictionary<int, double>();
            for (int y = 0; y < _windowSize.Height; y++)
                _distLookUp.Add(y, _windowSize.Height / (2.0 * y - _windowSize.Height));
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            //get fps
            _timeFromLastUpdate += e.Time;
            if (_timeFromLastUpdate >= 1 / TARGET_UPDATES_PER_SECOND)
            {
                UpdatePlayerPosition();

                _timeFromLastUpdate = 0;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _fps = 1 / e.Time;
            Console.Clear();
            Console.WriteLine("FPS: " + Math.Round(_fps, 2));
            Console.WriteLine("Rendering duration: " + _renderingCalculation + "ms");

            //render
            Render();
        }

        protected void KeyboardKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.W)
                _player.MovingState = MovingState.Forward;

            if (e.Key == Key.S)
                _player.MovingState = MovingState.Backward;

            if (e.Key == Key.D)
                _player.TurningState = TurningState.Right;

            if (e.Key == Key.A)
                _player.TurningState = TurningState.Left;
        }

        protected void KeyboardKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.A || e.Key == Key.D)
                _player.TurningState = TurningState.Idle;

            if (e.Key == Key.W || e.Key == Key.S)
                _player.MovingState = MovingState.Idle;
        }

        private void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.Black);

            DrawMap();

            SwapBuffers();
        }

        private void DrawMap()
        {
            Stopwatch renderTime = Stopwatch.StartNew();

            int[] spriteOrder = new int[NUM_SPRITES];
            double[] spriteDistance = new double[NUM_SPRITES];

            for (int x = 0; x < _windowSize.Width; x++)
            {
                DrawVerticalLine(x, spriteOrder, spriteDistance);
            }
            //SPRITE CASTING
            DrawSprites(spriteOrder, spriteDistance);

            _renderingCalculation = renderTime.ElapsedMilliseconds;
            renderTime.Restart();

            Draw.DrawPixels(_drawingBuffer);
            Array.Clear(_drawingBuffer, 0, _drawingBuffer.Length);
            Array.Clear(_zBuffer, 0, _zBuffer.Length);
        }

        private void DrawVerticalLine(int x, int[] spriteOrder, double[] spriteDistance)
        {
            double cameraX = 2 * x / (double)_windowSize.Width - 1; //x-coordinate in camera space
            Ray ray = new Ray(_player.PosX, _player.PosY, (_player.DirX + _camera.PlaneX * cameraX), (_player.DirY + _camera.PlaneY * cameraX));

            double perpWallDist;
            Point step = FindRayCastHit(ray);
            
            //Calculate distance projected on camera direction
            if (ray.Side == 0) perpWallDist = (ray.MapX - ray.RayPosX + (1 - step.X) / 2) / ray.RayDirX;
            else perpWallDist = (ray.MapY - ray.RayPosY + (1 - step.Y) / 2) / ray.RayDirY;

            //Height of line to draw on screen
            int lineHeight = (int)(_windowSize.Height / perpWallDist);

            //calculate lowest and highest pixel
            int drawStart = -lineHeight / 2 + _windowSize.Height / 2;
            if (drawStart < 0) drawStart = 0;
            int drawEnd = lineHeight / 2 + _windowSize.Height / 2;
            if (drawEnd >= _windowSize.Height) drawEnd = _windowSize.Height - 1;

            //TEXTURING CALCULATIONS
            int texNum = _worldMap[ray.MapX, ray.MapY] - 1; // -1 to use the index 0 of textures

            double wallX; //exact wall hit
            if (ray.Side == 0) wallX = ray.RayPosY + perpWallDist * ray.RayDirY;
            else wallX = ray.RayPosX + perpWallDist * ray.RayDirX;
            wallX -= Math.Floor(wallX);

            //x coord for texture
            int texX = (int)(wallX * (double)_textures.TexturesWidth);
            if (ray.Side == 0 && ray.RayDirX > 0) texX = _textures.TexturesWidth - texX - 1;
            if (ray.Side == 1 && ray.RayDirY < 0) texX = _textures.TexturesWidth - texX - 1;

            for (int y = drawStart; y < drawEnd; y++)
            {
                long d = y * 256 - _windowSize.Height * 128 + lineHeight * 128; //magic code that gets the right color -.-
                int texY = (int)((d * _textures.TexturesHeight) / lineHeight) / 256;
                Pixel pixelToDraw = new Pixel();
                pixelToDraw.X = x;
                pixelToDraw.Y = y;
                pixelToDraw.Color = _textures[texNum][_textures.TexturesHeight * texY + texX].Color;
                if (ray.Side == 1) pixelToDraw.Color = Color.FromArgb(pixelToDraw.Color.R / 2, pixelToDraw.Color.G / 2, pixelToDraw.Color.B / 2);
                _drawingBuffer[(_windowSize.Height * x) + y] = pixelToDraw;
            }

            //ZBUFFER FOR SRPITE CASTING
            _zBuffer[x] = perpWallDist;

            //FLOOR CASTING
            DrawPlanes(x, ray, wallX, perpWallDist, drawEnd, _drawingBuffer);
        }

        private void DrawPlanes(int x, Ray ray, double wallX, double perpWallDist, int drawEnd, Pixel[] drawingBuffer)
        {
            double floorXWall;
            double floorYWall; //x, y position of the floor texel at the bottom of the wall

            //4 different wall directions possible
            if (ray.Side == 0 && ray.RayDirX > 0)
            {
                floorXWall = ray.MapX;
                floorYWall = ray.MapY + wallX;
            }
            else if (ray.Side == 0 && ray.RayDirX < 0)
            {
                floorXWall = ray.MapX + 1.0;
                floorYWall = ray.MapY + wallX;
            }
            else if (ray.Side == 1 && ray.RayDirY > 0)
            {
                floorXWall = ray.MapX + wallX;
                floorYWall = ray.MapY;
            }
            else
            {
                floorXWall = ray.MapX + wallX;
                floorYWall = ray.MapY + 1.0;
            }

            double distWall;
            double distPlayer;
            double currentDist;

            distWall = perpWallDist;
            distPlayer = 0.0;

            if (drawEnd < 0) drawEnd = _windowSize.Height; //safety

            //draw the floor from drawEnd to the bottom of the screen
            for (int y = drawEnd + 1; y < _windowSize.Height; y++)
            {
                currentDist = _distLookUp[y]; //using precalculated lookuptable

                double weight = (currentDist - distPlayer) / (distWall - distPlayer);

                double currentFloorX = weight * floorXWall + (1.0 - weight) * _player.PosX;
                double currentFloorY = weight * floorYWall + (1.0 - weight) * _player.PosY;

                int floorTexX, floorTexY;
                floorTexX = (int)((currentFloorX * _textures.TexturesWidth) % _textures.TexturesWidth);
                floorTexY = (int)((currentFloorY * _textures.TexturesHeight) % _textures.TexturesHeight);

                Pixel pixelToDraw = new Pixel();
                pixelToDraw.X = x;
                pixelToDraw.Y = y;
                pixelToDraw.Color = _textures[6][_textures.TexturesWidth * floorTexY + floorTexX].Color;
                drawingBuffer[_windowSize.Height * x + y] = pixelToDraw;

                pixelToDraw = new Pixel();
                pixelToDraw.X = x;
                pixelToDraw.Y = _windowSize.Height - y;
                pixelToDraw.Color = _textures[1][_textures.TexturesWidth * floorTexY + floorTexX].Color;
                drawingBuffer[_windowSize.Height * x + (_windowSize.Height - y)] = pixelToDraw;
            }
        }

        private Point FindRayCastHit(Ray ray)
        {
            //length of ray
            double sideDistX;
            double sideDistY;

            //what direction to step in
            Point step = new Point();

            int hit = 0; //was there a wall hit?

            //calculate step and initial sideDist
            if (ray.RayDirX < 0)
            {
                step.X = -1;
                sideDistX = (ray.RayPosX - ray.MapX) * ray.DeltaDistX;
            }
            else
            {
                step.X = 1;
                sideDistX = (ray.MapX + 1.0 - ray.RayPosX) * ray.DeltaDistX;
            }
            if (ray.RayDirY < 0)
            {
                step.Y = -1;
                sideDistY = (ray.RayPosY - ray.MapY) * ray.DeltaDistY;
            }
            else
            {
                step.Y = 1;
                sideDistY = (ray.MapY + 1.0 - ray.RayPosY) * ray.DeltaDistY;
            }
            //DDA
            while (hit == 0)
            {
                //jump to next map square
                if (sideDistX < sideDistY)
                {
                    sideDistX += ray.DeltaDistX;
                    ray.MapX += step.X;
                    ray.Side = 0;
                }
                else
                {
                    sideDistY += ray.DeltaDistY;
                    ray.MapY += step.Y;
                    ray.Side = 1;
                }
                //Check if ray has hit a wall
                if (_worldMap[ray.MapX, ray.MapY] > 0) hit = 1;
            }
            return step;
        }

        private void DrawSprites(int[] spriteOrder, double[] spriteDistance)
        {
            //sort sprites from far to close
            for (int i = 0; i < NUM_SPRITES; i++)
            {
                spriteOrder[i] = i;
                spriteDistance[i] = ((_player.PosX - _sprites[i].X) * (_player.PosX - _sprites[i].X) + (_player.PosY - _sprites[i].Y) * (_player.PosY - _sprites[i].Y));
            }
            SpriteSort(spriteOrder, spriteDistance, NUM_SPRITES);

            //projection and draw
            for (int i = 0; i < NUM_SPRITES; i++)
            {
                //translate sprite position
                double spriteX = _sprites[spriteOrder[i]].X - _player.PosX;
                double spriteY = _sprites[spriteOrder[i]].Y - _player.PosY;

                double invDet = 1.0 / (_camera.PlaneX * _player.DirY - _player.DirX * _camera.PlaneY);

                double transformX = invDet * (_player.DirY * spriteX - _player.DirX * spriteY);
                double transformY = invDet * (-_camera.PlaneY * spriteX + _camera.PlaneX * spriteY);

                int spriteScreenX = (int)((_windowSize.Width / 2) * (1 + transformX / transformY));

                //calculate height of the sprite on screen
                int spriteHeight = Math.Abs((int)(_windowSize.Height / (transformY)));
                int drawStartY = -spriteHeight / 2 + _windowSize.Height / 2;
                if (drawStartY < 0) drawStartY = 0;
                int drawEndY = spriteHeight / 2 + _windowSize.Height / 2;
                if (drawEndY >= _windowSize.Height) drawEndY = _windowSize.Height - 1;

                //calculate width of the sprite
                int spriteWidth = Math.Abs((int)(_windowSize.Height / (transformY)));
                int drawStartX = -spriteWidth / 2 + spriteScreenX;
                if (drawStartX < 0) drawStartX = 0;
                int drawEndX = spriteWidth / 2 + spriteScreenX;
                if (drawEndX >= _windowSize.Width) drawEndX = _windowSize.Width - 1;

                //loop through every vertical stripe
                for (int stripe = drawStartX; stripe < drawEndX; stripe++)
                {
                    int texX = (int)(256 * (stripe - (-spriteWidth / 2 + spriteScreenX)) * _textures.TexturesWidth / spriteWidth) / 256;

                    //1) it's in front of camera plane so you don't see things behind you
                    //2) it's on the screen (left)
                    //3) it's on the screen (right)
                    //4) ZBuffer, with perpendicular distance
                    if (transformY > 0 && stripe > 0 && stripe < _windowSize.Width && transformY < _zBuffer[stripe])
                    {
                        for (int y = drawStartY; y < drawEndY; y++) //for every pixel of the current stripe
                        {
                            long d = (y) * 256 - _windowSize.Height * 128 + spriteHeight * 128; //magic code that gets the right color -.-
                            int texY = (int)((d * _textures.TexturesHeight) / spriteHeight) / 256;
                            Pixel pixelToDraw = new Pixel();
                            pixelToDraw.Y = y;
                            pixelToDraw.X = stripe;
                            pixelToDraw.Color = _textures[_sprites[spriteOrder[i]].Texture][_textures.TexturesWidth * texY + texX].Color; //get current color from the texture
                            if (pixelToDraw.Color != Color.FromArgb(0, 0, 0)) _drawingBuffer[_windowSize.Height * stripe + y] = pixelToDraw;
                        }
                    }
                }
            }
        }

        private void UpdatePlayerPosition()
        {
            //update player pos
            if (_player.MovingState == MovingState.Forward)
            {
                if (_worldMap[(int)(_player.PosX + _player.DirX * MOVEMENT_SPEED), (int)_player.PosY] == 0) _player.PosX += _player.DirX * MOVEMENT_SPEED;
                if (_worldMap[(int)_player.PosX, (int)(_player.PosY + _player.DirY * MOVEMENT_SPEED)] == 0) _player.PosY += _player.DirY * MOVEMENT_SPEED;
            }

            if (_player.MovingState == MovingState.Backward)
            {
                if (_worldMap[(int)(_player.PosX - _player.DirX * MOVEMENT_SPEED), (int)_player.PosY] == 0) _player.PosX -= _player.DirX * MOVEMENT_SPEED;
                if (_worldMap[(int)_player.PosX, (int)(_player.PosY - _player.DirY * MOVEMENT_SPEED)] == 0) _player.PosY -= _player.DirY * MOVEMENT_SPEED;
            }

            if (_player.TurningState == TurningState.Right)
            {
                double oldDirX = _player.DirX;
                _player.DirX = _player.DirX * Math.Cos(-ROT_SPEED) - _player.DirY * Math.Sin(-ROT_SPEED);
                _player.DirY = oldDirX * Math.Sin(-ROT_SPEED) + _player.DirY * Math.Cos(-ROT_SPEED);
                double oldPlaneX = _camera.PlaneX;
                _camera.PlaneX = _camera.PlaneX * Math.Cos(-ROT_SPEED) - _camera.PlaneY * Math.Sin(-ROT_SPEED);
                _camera.PlaneY = oldPlaneX * Math.Sin(-ROT_SPEED) + _camera.PlaneY * Math.Cos(-ROT_SPEED);
            }

            if (_player.TurningState == TurningState.Left)
            {
                double oldDirX = _player.DirX;
                _player.DirX = _player.DirX * Math.Cos(ROT_SPEED) - _player.DirY * Math.Sin(ROT_SPEED);
                _player.DirY = oldDirX * Math.Sin(ROT_SPEED) + _player.DirY * Math.Cos(ROT_SPEED);
                double oldPlaneX = _camera.PlaneX;
                _camera.PlaneX = _camera.PlaneX * Math.Cos(ROT_SPEED) - _camera.PlaneY * Math.Sin(ROT_SPEED);
                _camera.PlaneY = oldPlaneX * Math.Sin(ROT_SPEED) + _camera.PlaneY * Math.Cos(ROT_SPEED);
            }
        }

        private void SpriteSort(int[] order, double[] dist, int amount)
        {
            int gap = amount;
            bool swapped = false;
            while (gap > 1 || swapped)
            {
                gap = (gap * 10) / 13; //shrink based on scale
                if (gap == 9 || gap == 10) gap = 11;
                if (gap < 1) gap = 1;
                swapped = false;
                for (int i = 0; i < amount - gap; i++)
                {
                    int j = i + gap;
                    if (dist[i] < dist[j])
                    {
                        dist.SwapValues(i, j);
                        order.SwapValues(i, j);
                        swapped = true;
                    }
                }
            }
        }
    }
}
