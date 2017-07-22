using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.GFX.GUI
{
    public abstract class Screen : Engine.HandleTimerBase
    {
        protected GUI gui;

        protected Screen(GUI parent)
        {
            this.gui = parent;
        }

        public abstract void Draw(SpriteBatch sb);
    }
}
