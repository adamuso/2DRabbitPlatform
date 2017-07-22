using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.GFX
{
    public class Tile : Engine.ICollisionable
    {
        public const int STANDARD_GTILE_WIDTH = 32;
        public const int STANDARD_GTILE_HEIGHT = 32;
        private static readonly Rectangle COLLISION_AREA = new Rectangle(0, 0, STANDARD_GTILE_WIDTH, STANDARD_GTILE_HEIGHT);

        TileSet tileSet;
        TileMask mask;
        uint tileId;

        public Tile(TileSet tileSet, TileMask mask, uint tileId)
        {
            this.tileSet = tileSet;
            this.mask = mask;
            this.tileId = tileId;
        }

        public bool isEmpty { get { return tileId < 0; } }

        public Rectangle Source { get { return new Rectangle((int)(tileId % tileSet.Width * tileSet.Info.TileWidth), (int)(tileId / tileSet.Width * tileSet.Info.TileHeight), tileSet.Info.TileWidth, tileSet.Info.TileHeight); } }

        public int Width { get { return STANDARD_GTILE_WIDTH; } }

        public int Height { get { return STANDARD_GTILE_HEIGHT; } }

        public Mask Mask { get { return mask; } }

        public bool IsFlipped { get { return false; } }

        public Rectangle CollisionArea { get { return COLLISION_AREA; } }
    }
}
