using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.GFX.GUI
{
    public class DebugScreen : Screen
    {
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;
        Point pos;

        public DebugScreen(GUI gui, Point position)
            : base(gui)
        {
            this.pos = position;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            frameCounter++;

            string fps = string.Format("FPS: {0}", frameRate);

            sb.DrawString(gui.Game.DefaultFont, fps, new Vector2(pos.X + 1, pos.Y + 1), Color.Black);
            sb.DrawString(gui.Game.DefaultFont, fps,  new Vector2(pos.X, pos.Y), Color.White);
        }
    }
}
