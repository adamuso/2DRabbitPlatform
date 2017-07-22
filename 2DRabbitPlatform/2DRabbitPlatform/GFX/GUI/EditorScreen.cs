using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _2DRabbitPlatform.Engine.Editor;

namespace _2DRabbitPlatform.GFX.GUI
{
    public class EditorScreen : Screen
    {
        int tilemenu_startx, tilemenu_starty;
        PatternRectangle pattern;
        bool patterndown;

        public EditorScreen(GUI parent)
            : base(parent)
        {
            parent.InputManager.MouseManager.MouseWheel += new EventHandler<Engine.Input.MouseEvent>(MouseManager_MouseWheel);
            parent.InputManager.MouseManager.MouseDown += new EventHandler<Engine.Input.MouseEvent>(MouseManager_MouseDown);
            parent.InputManager.MouseManager.MouseUp += new EventHandler<Engine.Input.MouseEvent>(MouseManager_MouseUp);
        }

        void MouseManager_MouseUp(object sender, Engine.Input.MouseEvent e)
        {
            if (patterndown)
            {
                patterndown = false;
                pattern = PatternRectangle.Empty;
            }
        }

        void MouseManager_MouseDown(object sender, Engine.Input.MouseEvent e)
        {
            int mx = (int)e.Position.X - (gui.Game.GraphicsDevice.Viewport.Width - 192);

            if (mx > 0 && !patterndown)
            {
                pattern = new PatternRectangle(mx / 32 + TileMenu_StartX, (int)e.Position.Y / 32 + TileMenu_StartY); 

                patterndown = true;
            }
        }

        void MouseManager_MouseWheel(object sender, Engine.Input.MouseEvent e)
        {
            Console.WriteLine(e.WheelDelta);
            TileMenu_StartX += e.WheelDelta;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            TileSet tileset = gui.Game.CurrentWorld.Level.TileSet;

            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);

            for (int y = 0; y < tileset.Height - TileMenu_StartY; y++)
                for (int x = 0; x < tileset.Width - TileMenu_StartX; x++)
                {
                    Color c = Color.White;

                    if (new Rectangle(gui.Game.GraphicsDevice.Viewport.Width - 32 * 6 + x * 32, y * 32, 32, 32).Contains((int)gui.InputManager.MousePosition.X, (int)gui.InputManager.MousePosition.Y))
                        c = Color.Gray;

                    if (pattern.contains(x + TileMenu_StartX, y + TileMenu_StartY))
                        c = Color.Red;

                    sb.Draw(tileset.Texture, new Vector2(gui.Game.GraphicsDevice.Viewport.Width - 32 * 6 + x * 32, y * 32), tileset[x + TileMenu_StartX, y + TileMenu_StartY].Source, c);
                }
           
            sb.End();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
            base.Update(gt);

            if (patterndown)
            {
                int mx = (int)gui.InputManager.MouseLocation.X - (gui.Game.GraphicsDevice.Viewport.Width - 192);

                if (mx > 0)
                {
                    pattern.setSecond(mx / 32 + TileMenu_StartX, (int)gui.InputManager.MouseLocation.Y / 32 + TileMenu_StartY);
                }
            }
        }

        int TileMenu_StartX { get { return tilemenu_startx; } set { if (value > gui.Game.CurrentWorld.Level.TileSet.Width - 6)  tilemenu_startx = (int)gui.Game.CurrentWorld.Level.TileSet.Width - 6; else if (value < 0) tilemenu_startx = 0; else tilemenu_startx = value; } }
        int TileMenu_StartY { get { return tilemenu_starty; } set { if (value > gui.Game.CurrentWorld.Level.TileSet.Height - 10)  tilemenu_startx = (int)gui.Game.CurrentWorld.Level.TileSet.Height - 10; else if (value < 0) tilemenu_starty = 0; else tilemenu_starty = value; } }
    }
}
