using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace _2DRabbitPlatform.Engine.Editor
{
    public class EditorWorld : World
    {
        int tiledata;
        int layeredit;
        bool normaldraw;
        Viewport lefthalf, righthalf, def;

        public EditorWorld(RabbitPlatform instance)
            : base(instance)
        {
            InputManager.MouseManager.MouseWheel += new EventHandler<Input.MouseEvent>(MouseManager_MouseWheel);
            InputManager.MouseManager.MouseDown += new EventHandler<Input.MouseEvent>(MouseManager_MouseDown);
            InputManager.KeyboardManager.KeyDown += new EventHandler<Input.KeyboardEvent>(KeyboardManager_KeyDown);
            tiledata = 0;
            layeredit = 0;
            normaldraw = false;

            def = instance.GraphicsDevice.Viewport;
            lefthalf = instance.GraphicsDevice.Viewport;
            lefthalf.Width /= 2;
            righthalf = instance.GraphicsDevice.Viewport;
            righthalf.Width /= 2;
            righthalf.X = lefthalf.Width;
        }

        void KeyboardManager_KeyDown(object sender, Input.KeyboardEvent e)
        {
            if (e.Key == Keys.PageUp)
            {
                if (layeredit < Level.RenderManager.Layers.Count - 1)
                {
                    layeredit++;
                }
                else
                    normaldraw = true;

                if(Level.RenderManager.getLayer(LayerEdit).HasMap)
                    WorldCamera.setLimit(0, 0, Level.RenderManager.getLayer(LayerEdit).Map.Width * GFX.Tile.STANDARD_GTILE_WIDTH - (int)Game.Resolution.Scene.X, Level.RenderManager.getLayer(LayerEdit).Map.Height * GFX.Tile.STANDARD_GTILE_HEIGHT - (int)Game.Resolution.Scene.Y);
            }
            else if (e.Key == Keys.PageDown)
            {
                if (layeredit > 0 && !normaldraw)
                    layeredit--;

                if (normaldraw)
                    normaldraw = false;

                if (Level.RenderManager.getLayer(LayerEdit).HasMap)
                    WorldCamera.setLimit(0, 0, Level.RenderManager.getLayer(LayerEdit).Map.Width * GFX.Tile.STANDARD_GTILE_WIDTH - (int)Game.Resolution.Scene.X, Level.RenderManager.getLayer(LayerEdit).Map.Height * GFX.Tile.STANDARD_GTILE_HEIGHT - (int)Game.Resolution.Scene.Y);
            }
            else if (e.Key == Keys.Enter)
            {
                Console.WriteLine("Wpisz argumenty mapy: W H XS YS WW HW AXS AYS");
                string[] args = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (args.Length == 8)
                {
                    GFX.MapLayer layer = Level.RenderManager.getLayer(LayerEdit);
                    layer.Map = new GFX.TileMap(int.Parse(args[0]), int.Parse(args[1]), Level.RenderManager.getEntityLayer().Map.TileSet);
                    layer.AutoXScroll = float.Parse(args[6]);
                    layer.AutoYScroll = float.Parse(args[7]);
                    layer.YSpeed = float.Parse(args[3]);
                    layer.XSpeed = float.Parse(args[2]);
                    layer.WidthWrap = bool.Parse(args[4]);
                    layer.HeightWrap = bool.Parse(args[5]);

                    Console.WriteLine("Stworzono mapę!");
                }
            }
            else if(e.Key == Keys.Insert)
                Engine.Files.LevelFile.saveLevel(Level, "TestWorld.glv", "TS.gts");
        }

        void MouseManager_MouseDown(object sender, Input.MouseEvent e)
        {
            if (e.Button == Input.MouseButton.RIGHT)
            {
                if(Level.RenderManager.getLayer(LayerEdit).HasMap)
                    Level.RenderManager.getLayer(LayerEdit).Map.setTile(getAppToWorld(e.Position), new ExtendedTileData(Level.RenderManager.getLayer(LayerEdit).Map.getTile(getAppToWorld(e.Position)).ID, !Level.RenderManager.getLayer(LayerEdit).Map.getTile(getAppToWorld(e.Position)).Flipped));
            }
        }

        void MouseManager_MouseWheel(object sender, Input.MouseEvent e)
        {
            Console.WriteLine(tiledata);

            tiledata += e.WheelDelta;

            if (tiledata < 0)
                tiledata = 0;
        }

        public override void Update(GameTime gt)
        {
            if(normaldraw)
                base.Update(gt);
            else
                Level.TileManager.Update(gt);

            //RenderManager.Update(gt);
            //EntityManager.UpdateEntities(gt);
            

            int fac = InputManager.KeyboardManager.State.IsKeyDown(Keys.RightShift) ? 3 : 1;

            if (InputManager.KeyboardManager.State.IsKeyDown(Keys.Left))
                WorldCamera.moveTranslation(-2 * fac, 0);
            if (InputManager.KeyboardManager.State.IsKeyDown(Keys.Right))
                WorldCamera.moveTranslation(2 * fac, 0);
            if (InputManager.KeyboardManager.State.IsKeyDown(Keys.Up))
                WorldCamera.moveTranslation(0, -2 * fac);
            if (InputManager.KeyboardManager.State.IsKeyDown(Keys.Down))
                WorldCamera.moveTranslation(0, 2 * fac);

            Vector2 position = new Vector2(InputManager.MouseManager.State.X, InputManager.MouseManager.State.Y);

            if (InputManager.MouseManager.State.LeftButton == ButtonState.Pressed)
                if(Level.RenderManager.getLayer(LayerEdit).HasMap)
                    Level.RenderManager.getLayer(LayerEdit).Map.getTile(getAppToWorld(position)).ID = tiledata;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            if (normaldraw)
            {
                sb.GraphicsDevice.Viewport = def;
                refreshCamera();
                base.Draw(sb);
            }
            else
            {
                sb.GraphicsDevice.Viewport = lefthalf;
                refreshCamera();

                Level.RenderManager.render(Level.RenderManager.getLayer(LayerEdit).Map);
                if (layeredit == Level.RenderManager.EntityLayerID)
                    EntityManager.DrawEntities(sb);

                sb.GraphicsDevice.Viewport = righthalf;

                base.Draw(sb);

                sb.GraphicsDevice.Viewport = lefthalf;
            }

            if (Level.RenderManager.getLayer(LayerEdit).HasMap)
            {
                if (!Level.RenderManager.getLayer(LayerEdit).Map.TileSet[tiledata].isEmpty)
                {
                    Vector2 position = new Vector2(InputManager.MouseManager.State.X, InputManager.MouseManager.State.Y);
                    Location loc = getAppToWorld(position);

                    if (loc.getTileX() < Level.RenderManager.getLayer(LayerEdit).Map.Width &&
                        loc.getTileY() < Level.RenderManager.getLayer(LayerEdit).Map.Height)
                        sb.Draw(Level.RenderManager.getLayer(LayerEdit).Map.TileSet.Texture, new Rectangle(loc.getTileX() * 32, loc.getTileY() * 32, 32, 32), Level.RenderManager.getLayer(LayerEdit).Map.TileSet[tiledata].Source, new Color(128, 128, 128, 128));
                }
            }
        }

        int LayerEdit { get { return normaldraw ? Level.RenderManager.EntityLayerID : layeredit; } }
    }
}
