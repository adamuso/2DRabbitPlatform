using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.GFX
{
    public class MapLayer : SpriteBatch
    {
        protected Engine.World world;
        private float depth;
        private float xspeed, yspeed;
        private bool widthWrap, heightWrap;
        protected Matrix extraTransform;
        float autoX, autoY;
        TileMap map;
        float scrollX, scrollY;
        bool autoscroll;

        public MapLayer(Engine.World world, TileMap map, float depth, bool autoscroll = true) 
            : base(world.Game.GraphicsDevice)
        {
            this.world = world;
            this.depth = depth;
            this.xspeed = 1f;
            this.yspeed = 1f;
            this.widthWrap = false;
            this.heightWrap = false;
            this.map = map;
            this.autoX = 0f;
            this.autoY = 0f;
            this.scrollX = 0f;
            this.scrollY = 0f;
            this.autoscroll = autoscroll;
            extraTransform = Matrix.Identity;
        }

        public new void Begin()
        {
            base.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, Wrap ? SamplerState.PointWrap : SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, world.WorldCamera.getProjection(new Microsoft.Xna.Framework.Vector2(xspeed, yspeed)) * extraTransform * world.Game.Resolution.Projection);
        }

        public override string ToString()
        {
            return "Depth: " + depth + ", XS: " + xspeed + ", YS: " + yspeed;
        }

        public void Update(GameTime gt)
        {
        if (autoscroll)
            {
                if (autoX != 0 || autoY != 0)
                {
                    extraTransform = Matrix.CreateTranslation(scrollX, scrollY, 0f);                // Extra transform for our SpriteBatch

                    if (WidthWrap)
                    {
                        float fac = -(scrollX / Math.Abs(scrollX));

                        if (Math.Abs(scrollX) > map.Width * Tile.STANDARD_GTILE_WIDTH * (2 << 8))   // Absorbs blinking of auto scroll, (2 << 8) means that after 512 repeat the numbers will refresh to first state
                            scrollX += map.Width * Tile.STANDARD_GTILE_WIDTH * (2 << 8) * fac;      // fac - if we're moving something to the right it'll have -1 value, otherwise it'll be 1
                    }

                    if (HeightWrap)
                    {
                        float fac = -(scrollY / Math.Abs(scrollY));

                        if (Math.Abs(scrollY) > map.Height * Tile.STANDARD_GTILE_HEIGHT * (2 << 8)) // Absorbs blinking of auto scroll, (2 << 8) means that after 512 repeat the numbers will refresh to first state
                            scrollY += map.Height * Tile.STANDARD_GTILE_HEIGHT * (2 << 8) * fac;    // fac - if we're moving something down it'll have -1 value, otherwise it'll be 1
                    }
                    
                    scrollX += autoX * world.Game.getTimes(gt);                                     // Adding x-axis scroll over time
                    scrollY += autoY * world.Game.getTimes(gt);                                     // Adding y-axis scroll over time
                }
            }
        }

        public virtual bool isOnScreen(Vector2 point)
        {
            Vector2 buf = new Vector2((world.WorldCamera.Translation.X - Translation.X) + point.X, (world.WorldCamera.Translation.Y - Translation.Y) + point.Y);
            Point temp = new Point((int)buf.X, (int)buf.Y);
            return world.WorldCamera.Bounds.Contains(temp);
        }

        public virtual bool isOnScreen(Rectangle rectangle)
        {
            Vector2 buf = new Vector2((world.WorldCamera.Translation.X - Translation.X) + rectangle.X, (world.WorldCamera.Translation.Y - Translation.Y) + rectangle.Y);
            Rectangle ret = Rectangle.Intersect(world.WorldCamera.Bounds, new Rectangle((int)buf.X, (int)buf.Y, rectangle.Width, rectangle.Height));
            ret.X -= (int)buf.X;
            ret.Y -= (int)buf.Y;
            return world.WorldCamera.Bounds.Intersects(ret) || world.WorldCamera.Bounds.Contains(ret);
        }

        public Rectangle overlapWithScreen(Rectangle rectangle)
        {
            Vector2 buf = new Vector2((world.WorldCamera.Translation.X - Translation.X) + rectangle.X + scrollX, (world.WorldCamera.Translation.Y - Translation.Y) + rectangle.Y + scrollY);
            Rectangle ret = Rectangle.Intersect(world.WorldCamera.Bounds, new Rectangle((int)buf.X, (int)buf.Y, rectangle.Width, rectangle.Height));
            ret.X -= (int)buf.X;
            ret.Y -= (int)buf.Y;
            return ret;
        }

        public Engine.World World { get { return world; } }
        private bool Wrap { get { return widthWrap || heightWrap; } }
        public bool WidthWrap { get { return widthWrap; } set { widthWrap = value; } }
        public bool HeightWrap { get { return heightWrap; } set { heightWrap = value; } }
        public float XSpeed { get { return xspeed; } set { xspeed = value; } }
        public float YSpeed { get { return yspeed; } set { yspeed = value; } }
        public float Depth { get { return depth; } }
        public float AutoXScroll { get { return autoX; } set { autoX = value; } }
        public float AutoYScroll { get { return autoY; } set { autoY = value; } }
        public bool AutoScrollEnabled { get { return autoscroll; } set { autoscroll = value; } }
        public TileMap Map { get { return map; } set { map = value; } }
        public bool HasMap { get { return map != null; } }
        public float XScroll { get { return scrollX; } set { if (!autoscroll) scrollX = value; else throw new NotSupportedException("Autoscroll is enabled!"); } }
        public float YScroll { get { return scrollY; } set { if (!autoscroll) scrollY = value; else throw new NotSupportedException("Autoscroll is enabled!"); } }
        public Vector2 Translation { get { return new Vector2(world.WorldCamera.Translation.X * xspeed, world.WorldCamera.Translation.Y * yspeed); } }
    }
}
