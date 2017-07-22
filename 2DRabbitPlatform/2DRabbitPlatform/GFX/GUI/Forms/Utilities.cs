using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.GFX.GUI.Forms
{
    public static class Utilities
    {
        public static Vector2 centerText(Rectangle bounds, Vector2 fontMeasure, float scale)
        {
            fontMeasure = new Vector2(fontMeasure.X * scale, fontMeasure.Y * scale);
            Vector2 center = new Vector2(bounds.X + bounds.Width / 2 - fontMeasure.X / 2, bounds.Y + bounds.Height / 2 - fontMeasure.Y / 2);

            return center;
        }
    }
}
