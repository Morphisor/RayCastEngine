using OpenTK;
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
using System.Runtime;
using System.Diagnostics;
using RayCast.Models;
using RayCast.Models.BaseModels;
using RayCast.Models.Enums;

namespace RayCast.Core
{
    public class Engine : GameWindow
    {

        private static double TARGET_UPDATES_PER_SECOND = 20;
        private static double MOVEMENT_SPEED = 0.1;
        private static double ROT_SPEED = 0.15;

        private int[,] _worldMap;
        private int _spriteNumber;

        private readonly Size _windowSize;
        private readonly Size _viewPort;
        private readonly Point _mimiMapPosition;

        private double _timeFromLastUpdate;
        private double _fps;
        private long _renderingCalculation;

        private ResourceManager _resourceManager;

        private Textures _textures;

        private Pixel[] _drawingBuffer;
        private double[] _zBuffer;

        private Player _player;

        private List<Prop> _props;

        private Dictionary<int, double> _distLookUp;

        public Engine(Size windowSize) : base(windowSize.Width, windowSize.Height)
        {
            _windowSize = windowSize;
            _viewPort = new Size(_windowSize.Width - 260, windowSize.Height);
            _mimiMapPosition = new Point(_viewPort.Width + 10, 10);
        }

        protected override void OnLoad(EventArgs e)
        {
            _drawingBuffer = new Pixel[_viewPort.Width * _viewPort.Height + 1];
            _zBuffer = new double[_viewPort.Width];

            _resourceManager = ResourceManager.Instance;
            _resourceManager.ReadResourceFile("testlevel");
            _worldMap = _resourceManager.WorldMap;
            _textures = _resourceManager.Textures;

            _player = new Player();
            _player.Position = new Position(22, 12, -1, 0);
            _player.Movement = new Movement();
            _player.Weapon = new Weapon(200, 200, new int[] { 15 }, _windowSize.Width - 260);
            _player.Camera = new Camera(0, 0.66);



            _props = new List<Prop>();
            foreach (var prop in _resourceManager.Props)
            {
                var propToAdd = new Prop();
                propToAdd.Position = new Position(prop.Position.PosX, prop.Position.PosY, 0, 0);
                propToAdd.Sprite = new Sprite(prop.Sprite.Texture);

                if (_resourceManager.AnimatedSprites.ContainsKey(Array.IndexOf(_resourceManager.Props, prop)))
                {
                    propToAdd = new AnimatedProp();
                    propToAdd.Position = new Position(prop.Position.PosX, prop.Position.PosY, 0, 0);
                    propToAdd.Sprite = new Sprite(prop.Sprite.Texture);
                    (propToAdd as AnimatedProp).Animation = new Animation(_resourceManager.AnimatedSprites[Array.IndexOf(_resourceManager.Props, prop)], AnimationType.Movement);
                }

                _props.Add(propToAdd);
            }

            AnimatedProp enemyProp = _props[0] as AnimatedProp;
            _props[0] = new Enemy();
            _props[0].Position = enemyProp.Position;
            _props[0].Sprite = enemyProp.Sprite;
            (_props[0] as Enemy).Animation = enemyProp.Animation;

            _spriteNumber = _props.Count;

            //setup input
            Keyboard.KeyDown += new EventHandler<KeyboardKeyEventArgs>(KeyboardKeyDown);
            Keyboard.KeyUp += new EventHandler<KeyboardKeyEventArgs>(KeyboardKeyUp);

            //setup drawing mode
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, _windowSize.Width, _windowSize.Height, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Modelview);

            //init lookup
            _distLookUp = new Dictionary<int, double>();
            for (int y = 0; y < _viewPort.Height; y++)
                _distLookUp.Add(y, _viewPort.Height / (2.0 * y - _viewPort.Height));
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            //get fps
            _timeFromLastUpdate += e.Time;
            if (_timeFromLastUpdate >= 1 / TARGET_UPDATES_PER_SECOND)
            {
                UpdatePlayerPosition();

                foreach (var prop in _props)
                {
                    if (prop is Enemy)
                    {
                        prop.Update(new object[] { _player, _worldMap });
                    }
                    else
                    {
                        prop.Update();
                    }
                }

                _timeFromLastUpdate = 0;
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            _fps = 1 / e.Time;

            Title = $"RayCastEngine - (FPS: {Math.Round(_fps, 2)}) (Rendering duration: {_renderingCalculation}ms)";

            Render();
        }

        protected void KeyboardKeyDown(object sender, KeyboardKeyEventArgs e)
        {

            var playerMovement = _player.Movement;
            var weapon = _player.Weapon;

            if (e.Key == Key.W)
                playerMovement.MovingState = MovingState.Forward;

            if (e.Key == Key.S)
                playerMovement.MovingState = MovingState.Backward;

            if (e.Key == Key.D)
                playerMovement.TurningState = TurningState.Right;

            if (e.Key == Key.A)
                playerMovement.TurningState = TurningState.Left;

            if (e.Key == Key.Space)
                weapon.IsShooting = true;

        }

        protected void KeyboardKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            var playerMovement = _player.Movement;
            var weapon = _player.Weapon;

            if (e.Key == Key.A || e.Key == Key.D)
                playerMovement.TurningState = TurningState.Idle;

            if (e.Key == Key.W || e.Key == Key.S)
                playerMovement.MovingState = MovingState.Idle;

            if (e.Key == Key.Space)
                weapon.IsShooting = false;
        }

        private void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.ClearColor(Color.Black);

            Stopwatch renderTime = Stopwatch.StartNew();

            DrawMap();
            DrawMinimap(_mimiMapPosition, 10);
            DrawWeapon();
            
            Draw.DrawPixels(_drawingBuffer);

            _renderingCalculation = renderTime.ElapsedMilliseconds;
            renderTime.Restart();

            SwapBuffers();
        }

        private void DrawWeapon()
        {
            var weapon = _player.Weapon;
            if (weapon.IsShooting)
                weapon.UpdateNextFrame();

            int startX = (_viewPort.Width / 2) - (weapon.Width / 2);
            int endX = (_viewPort.Width / 2) + (weapon.Width / 2);

            int startY = _viewPort.Height - weapon.Height;
            int endY = _viewPort.Height;

            int texY = 0;
            int texX = 0;

            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    Pixel pixelToDraw = new Pixel();
                    pixelToDraw.Y = y;
                    pixelToDraw.X = x;
                    pixelToDraw.Color = _textures[weapon.GetCurrentFrame()][weapon.Width * texY+ texX].Color; //get current color from the texture
                    if (pixelToDraw.Color != Color.FromArgb(152, 0, 136)) _drawingBuffer[_viewPort.Height * x + y] = pixelToDraw;
                    texY++;
                }
                texY = 0;
                texX++;
            }
        }

        private void DrawMinimap(Point miniMapPosition, int blockSize)
        {
            //draw map
            var playerPosition = _player.Position;
            Color blockColor = Color.White;
            for (int y = 0; y < _worldMap.GetLength(0); y++)
            {
                for (int x = 0; x < _worldMap.GetLength(1); x++)
                {
                    if (_worldMap[y, x] != 0)
                    {
                        int xStart = x * blockSize + miniMapPosition.X;
                        int yStart = y * blockSize + miniMapPosition.Y;
                        Draw.DrawRectangle(new Rectangle(xStart, yStart, blockSize, blockSize), Color.DarkRed);
                    }
                }
            }
            //draw player
            Point mapPlayer = new Point((int)Math.Floor((playerPosition.PosY * 64) / 64 * blockSize) + miniMapPosition.X, (int)Math.Floor((playerPosition.PosX * 64) / 64 * blockSize) + miniMapPosition.Y);
            Draw.DrawCircle(mapPlayer, blockSize, Color.Red);
        }

        private void DrawMap()
        {
            int[] spriteOrder = new int[_spriteNumber];
            double[] spriteDistance = new double[_spriteNumber];

            for (int x = 0; x < _viewPort.Width; x++)
            {
                DrawVerticalLine(x);
            }
            //SPRITE CASTING
            DrawSprites(spriteOrder, spriteDistance);
        }

        private void DrawVerticalLine(int x)
        {
            var playerPosition = _player.Position;
            var camera = _player.Camera;

            double cameraX = 2 * x / (double)_viewPort.Width - 1; //x-coordinate in camera space
            Ray ray = new Ray(playerPosition.PosX, playerPosition.PosY, (playerPosition.DirX + camera.PlaneX * cameraX), (playerPosition.DirY + camera.PlaneY * cameraX));

            double perpWallDist;
            Point step = FindRayCastHit(ray);
            
            //Calculate distance projected on camera direction
            if (ray.Side == 0) perpWallDist = (ray.MapX - ray.RayPosX + (1 - step.X) / 2) / ray.RayDirX;
            else perpWallDist = (ray.MapY - ray.RayPosY + (1 - step.Y) / 2) / ray.RayDirY;

            //Height of line to draw on screen
            int lineHeight = (int)(_viewPort.Height / perpWallDist);

            //calculate lowest and highest pixel
            int drawStart = -lineHeight / 2 + _viewPort.Height / 2;
            if (drawStart < 0) drawStart = 0;
            int drawEnd = lineHeight / 2 + _viewPort.Height / 2;
            if (drawEnd >= _viewPort.Height) drawEnd = _viewPort.Height - 1;

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
                long d = y * 256 - _viewPort.Height * 128 + lineHeight * 128; //magic code that gets the right color -.-
                int texY = (int)((d * _textures.TexturesHeight) / lineHeight) / 256;
                Pixel pixelToDraw = new Pixel();
                pixelToDraw.X = x;
                pixelToDraw.Y = y;
                pixelToDraw.Color = _textures[texNum][_textures.TexturesHeight * texY + texX].Color;
                if (ray.Side == 1) pixelToDraw.Color = Color.FromArgb(pixelToDraw.Color.R / 2, pixelToDraw.Color.G / 2, pixelToDraw.Color.B / 2);
                _drawingBuffer[(_viewPort.Height * x) + y] = pixelToDraw;
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

            var playerPosition = _player.Position;

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

            if (drawEnd < 0) drawEnd = _viewPort.Height; //safety

            //draw the floor from drawEnd to the bottom of the screen
            for (int y = drawEnd + 1; y < _viewPort.Height; y++)
            {
                currentDist = _distLookUp[y]; //using precalculated lookuptable

                double weight = (currentDist - distPlayer) / (distWall - distPlayer);

                double currentFloorX = weight * floorXWall + (1.0 - weight) * playerPosition.PosX;
                double currentFloorY = weight * floorYWall + (1.0 - weight) * playerPosition.PosY;

                int floorTexX, floorTexY;
                floorTexX = (int)((currentFloorX * _textures.TexturesWidth) % _textures.TexturesWidth);
                floorTexY = (int)((currentFloorY * _textures.TexturesHeight) % _textures.TexturesHeight);

                Pixel pixelToDraw = new Pixel();
                pixelToDraw.X = x;
                pixelToDraw.Y = y;
                pixelToDraw.Color = _textures[6][_textures.TexturesWidth * floorTexY + floorTexX].Color;
                drawingBuffer[_viewPort.Height * x + y] = pixelToDraw;

                pixelToDraw = new Pixel();
                pixelToDraw.X = x;
                pixelToDraw.Y = _viewPort.Height - y;
                pixelToDraw.Color = _textures[1][_textures.TexturesWidth * floorTexY + floorTexX].Color;
                drawingBuffer[_viewPort.Height * x + (_viewPort.Height - y)] = pixelToDraw;
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
            var playerPosition = _player.Position;
            var camera = _player.Camera;

            //sort sprites from far to close
            for (int i = 0; i < _spriteNumber; i++)
            {
                spriteOrder[i] = i;
                var spriteComponents = _props[i].Position;
                spriteDistance[i] = ((playerPosition.PosX - spriteComponents.PosX) * (playerPosition.PosX - spriteComponents.PosX) + (playerPosition.PosY - spriteComponents.PosY) * (playerPosition.PosY - spriteComponents.PosY));
            }
            SpriteSort(spriteOrder, spriteDistance, _spriteNumber);

            //projection and draw
            for (int i = 0; i < _spriteNumber; i++)
            {

                var spriteComponent = _props[spriteOrder[i]];

                spriteComponent.Sprite.IsVisible = false;

                //translate sprite position
                double spriteX = spriteComponent.Position.PosX - playerPosition.PosX;
                double spriteY = spriteComponent.Position.PosY - playerPosition.PosY;

                double invDet = 1.0 / (camera.PlaneX * playerPosition.DirY - playerPosition.DirX * camera.PlaneY);

                double transformX = invDet * (playerPosition.DirY * spriteX - playerPosition.DirX * spriteY);
                double transformY = invDet * (-camera.PlaneY * spriteX + camera.PlaneX * spriteY);

                int spriteScreenX = (int)((_viewPort.Width / 2) * (1 + transformX / transformY));

                //calculate height of the sprite on screen
                int spriteHeight = (int)Math.Abs((long)(_viewPort.Height / (transformY)));
                int drawStartY = -spriteHeight / 2 + _viewPort.Height / 2;
                if (drawStartY < 0) drawStartY = 0;
                int drawEndY = spriteHeight / 2 + _viewPort.Height / 2;
                if (drawEndY >= _viewPort.Height) drawEndY = _viewPort.Height - 1;

                //calculate width of the sprite
                int spriteWidth = (int)Math.Abs((long)(_viewPort.Height / (transformY)));
                int drawStartX = -spriteWidth / 2 + spriteScreenX;
                if (drawStartX < 0) drawStartX = 0;
                int drawEndX = spriteWidth / 2 + spriteScreenX;
                if (drawEndX >= _viewPort.Width) drawEndX = _viewPort.Width - 1;

                //loop through every vertical stripe
                for (int stripe = drawStartX; stripe < drawEndX; stripe++)
                {
                    int texX = (int)(256 * (stripe - (-spriteWidth / 2 + spriteScreenX)) * _textures.TexturesWidth / spriteWidth) / 256;

                    //1) it's in front of camera plane so you don't see things behind you
                    //2) it's on the screen (left)
                    //3) it's on the screen (right)
                    //4) ZBuffer, with perpendicular distance
                    if (transformY > 0 && stripe > 0 && stripe < _viewPort.Width && transformY < _zBuffer[stripe])
                    {
                        spriteComponent.Sprite.IsVisible = true;
                        for (int y = drawStartY; y < drawEndY; y++) //for every pixel of the current stripe
                        {
                            long d = (y) * 256 - _viewPort.Height * 128 + spriteHeight * 128; //magic code that gets the right color -.-
                            int texY = (int)((d * _textures.TexturesHeight) / spriteHeight) / 256;
                            Pixel pixelToDraw = new Pixel();
                            pixelToDraw.Y = y;
                            pixelToDraw.X = stripe;
                            pixelToDraw.Color = _textures[spriteComponent.Sprite.Texture][_textures.TexturesWidth * texY + texX].Color; //get current color from the texture
                            if (pixelToDraw.Color != Color.FromArgb(152, 0, 136)) _drawingBuffer[_viewPort.Height * stripe + y] = pixelToDraw;
                        }

                        var weapon = _player.Weapon;
                        if (_props[spriteOrder[i]] is Enemy && weapon.IsShooting && stripe > weapon.HitMarginLeft && stripe < weapon.HitMarginRight )
                        {
                            //HIT
                        }
                    }
                }
            }
        }

        private void UpdatePlayerPosition()
        {
            var playerPosition = _player.Position;
            var playerMovement = _player.Movement;
            var camera = _player.Camera;

            //update player pos
            if (playerMovement.MovingState == MovingState.Forward)
            {
                if (_worldMap[(int)(playerPosition.PosX + playerPosition.DirX * MOVEMENT_SPEED), (int)playerPosition.PosY] == 0) playerPosition.PosX += playerPosition.DirX * MOVEMENT_SPEED;
                if (_worldMap[(int)playerPosition.PosX, (int)(playerPosition.PosY + playerPosition.DirY * MOVEMENT_SPEED)] == 0) playerPosition.PosY += playerPosition.DirY * MOVEMENT_SPEED;
            }

            if (playerMovement.MovingState == MovingState.Backward)
            {
                if (_worldMap[(int)(playerPosition.PosX - playerPosition.DirX * MOVEMENT_SPEED), (int)playerPosition.PosY] == 0) playerPosition.PosX -= playerPosition.DirX * MOVEMENT_SPEED;
                if (_worldMap[(int)playerPosition.PosX, (int)(playerPosition.PosY - playerPosition.DirY * MOVEMENT_SPEED)] == 0) playerPosition.PosY -= playerPosition.DirY * MOVEMENT_SPEED;
            }

            if (playerMovement.TurningState == TurningState.Right)
            {
                double oldDirX = playerPosition.DirX;
                playerPosition.DirX = playerPosition.DirX * Math.Cos(-ROT_SPEED) - playerPosition.DirY * Math.Sin(-ROT_SPEED);
                playerPosition.DirY = oldDirX * Math.Sin(-ROT_SPEED) + playerPosition.DirY * Math.Cos(-ROT_SPEED);
                double oldPlaneX = camera.PlaneX;
                camera.PlaneX = camera.PlaneX * Math.Cos(-ROT_SPEED) - camera.PlaneY * Math.Sin(-ROT_SPEED);
                camera.PlaneY = oldPlaneX * Math.Sin(-ROT_SPEED) + camera.PlaneY * Math.Cos(-ROT_SPEED);
            }

            if (playerMovement.TurningState == TurningState.Left)
            {
                double oldDirX = playerPosition.DirX;
                playerPosition.DirX = playerPosition.DirX * Math.Cos(ROT_SPEED) - playerPosition.DirY * Math.Sin(ROT_SPEED);
                playerPosition.DirY = oldDirX * Math.Sin(ROT_SPEED) + playerPosition.DirY * Math.Cos(ROT_SPEED);
                double oldPlaneX = camera.PlaneX;
                camera.PlaneX = camera.PlaneX * Math.Cos(ROT_SPEED) - camera.PlaneY * Math.Sin(ROT_SPEED);
                camera.PlaneY = oldPlaneX * Math.Sin(ROT_SPEED) + camera.PlaneY * Math.Cos(ROT_SPEED);
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
