using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Editor
{
    public struct PatternRectangle
    {
        int x1, y1, x2, y2;

        public PatternRectangle(int first_x, int first_y)
        {
            x1 = x2 = first_x;
            y1 = y2 = first_y;
        }

        public void setFirst(int x, int y)
        {
            x1 = x;
            y1 = y;
        }

        public void setSecond(int x, int y)
        {
            x2 = x;
            y2 = y;
        }

        public bool contains(int x, int y)
        {
            return x >= X && y >= Y && x <= X + Width && y <= Y + Height;
        }

        public int X { get { return Math.Min(x1, x2); } }
        public int Y { get { return Math.Min(y1, y2); } }
        public int Width { get { return Math.Abs(x1 - x2); } }
        public int Height { get { return Math.Abs(y1 - y2); } }

        public static readonly PatternRectangle Empty = new PatternRectangle(0, 0);
    }
}
