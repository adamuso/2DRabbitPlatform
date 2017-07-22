using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.GFX.GUI
{
    public class GameScreen : Screen
    {
        DebugScreen debug;

        public GameScreen(GUI parent) 
            : base(parent)
        {
            debug = new DebugScreen(parent, new Microsoft.Xna.Framework.Point(0, 0));
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, gui.Game.CurrentWorld.WorldCamera.Projection * gui.Game.Resolution.Projection);
            gui.Game.CurrentWorld.Draw(sb);
            sb.End();

            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, gui.Game.Resolution.Projection);
            debug.Draw(sb);
            //drawHealth(sb);
            sb.End();
        }

        public void drawHealth(SpriteBatch sb)
        {
            Texture2D text = gui.Game.TextureManager.gui_healthq;
            int startx = 800, starty = 10;

            for (int i = 0; i < gui.Game.CurrentWorld.Player.Health; i++)
            {
                sb.Draw(text, new Vector2(startx, starty) + new Vector2(17 * (i / 2) * 0.75f, 17 * (i % 2 == 1 ? 1 : 0) * 0.75f), null, Color.White, 0f, Vector2.Zero, 0.75f,
                        SpriteEffects.None | (i % 2 == 1 ? SpriteEffects.FlipVertically : SpriteEffects.None) | ((i / 2 + 1) % 2 == 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0.9f);

                sb.Draw(text, new Vector2(startx - 2, starty - 2) + new Vector2(17 * (i / 2) * 0.75f, 17 * (i % 2 == 1 ? 1 : 0) * 0.75f), null, Color.Black, 0f, Vector2.Zero, 0.75f,
                      SpriteEffects.None | (i % 2 == 1 ? SpriteEffects.FlipVertically : SpriteEffects.None) | ((i / 2 + 1) % 2 == 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 1f);
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
            base.Update(gt);

            gui.Game.CurrentWorld.Update(gt);
            debug.Update(gt);
        }
    }
}
