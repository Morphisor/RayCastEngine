using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayCast.Core.Primitives;

namespace RayCast.Core.Utils
{
    public static class Draw
    {
        public static void DrawPixel(int x, int y, Color color)
        {
            GL.Begin(PrimitiveType.Points);
            GL.Color3(color);
            GL.Vertex2(x, y);
            GL.End();
        }

        public static void DrawPixels(IEnumerable<Pixel> pixels)
        {
            GL.Begin(PrimitiveType.Points);
            Color current = Color.Empty;

            foreach (Pixel p in pixels)
            {
                if (p != null)
                {
                    if (p.Color != current)
                    {
                        current = p.Color;
                        GL.Color3(current);
                    }
                    GL.Vertex2(p.X, p.Y);
                }
            }
            GL.End();
        }

        public static int[] DrawToMemory(IEnumerable<Pixel> pixels)
        {
            int[] result = new int[pixels.Count()];
            int index = 0;

            foreach (Pixel p in pixels)
            {
                result[index] = (p != null) ? p.Color.ToArgb() : Color.Black.ToArgb();
                index++;
            }

            return result;
        }

        public static void DrawRectangle(Rectangle rectangle, Color color)
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(color);
            GL.Vertex2(rectangle.X, rectangle.Y);
            GL.Vertex2(rectangle.X + rectangle.Width, rectangle.Y);
            GL.Vertex2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
            GL.Vertex2(rectangle.X, rectangle.Y + rectangle.Height);
            GL.End();
        }

        public static void DrawCircle(Point location, int size, Color color)
        {
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color3(color);
            GL.Vertex2(location.X, location.Y);

            for (int angle = 0; angle <= 360; angle++)
            {
                double radianAngle = ((double)angle).ToRadians();
                GL.Vertex2(location.X + Math.Sin(radianAngle) * size / 2, location.Y + Math.Cos(radianAngle) * size / 2);
            }

            GL.End();
        }

        public static void DrawLine(Point start, Point end, Color color)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(color);
            GL.Vertex2(start.X, start.Y);
            GL.Vertex2(end.X, end.Y);
            GL.End();
        }
    }
}
