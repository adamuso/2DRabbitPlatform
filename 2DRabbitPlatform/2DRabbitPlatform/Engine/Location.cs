using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine
{
    public struct Location
    {
        Vector2 pos;
        World world;

        public Location(World world, float x, float y)
            : this(world, new Vector2(x, y)) { }

        public Location(World world, Vector2 vector)
        {
            this.world = world;
            pos = vector;
        }

        public float getDistance(Location l)
        {
            return (float)Math.Sqrt(Math.Pow(pos.X - l.X, 2) + Math.Pow(pos.Y - l.Y, 2));
        }

        public float getDirectionTo(Location l)
        {
            return (float)Math.Atan2(l.pos.Y - this.pos.Y, l.pos.X - this.pos.X);
        }

        public int getTileX()
        {
            return (int)pos.X / _2DRabbitPlatform.GFX.Tile.STANDARD_GTILE_WIDTH;
        }

        public int getTileY()
        {
            return (int)pos.Y / _2DRabbitPlatform.GFX.Tile.STANDARD_GTILE_HEIGHT;
        }

        public TileData getTile()
        {
            return world.Level.getTile(this);
        }

        public void move(float x, float y)
        {
            this.pos.X += x;
            this.pos.Y += y;
        }

        public Location offset(float x, float y)
        {
            return new Location(world, pos += new Vector2(x, y));
        }

        public static implicit operator Vector2(Location location)
        {
            return location.pos;
        }

        public Vector2 AppVector { get { return world.getStaticPosition(pos); } }
        public float X { get { return pos.X; } set { pos.X = value; } }
        public float Y { get { return pos.Y; } set { pos.Y = value; } }
        public int iX { get { return (int)pos.X; } }
        public int iY { get { return (int)pos.Y; } }
        public World World { get { return world; } }
    }
}
