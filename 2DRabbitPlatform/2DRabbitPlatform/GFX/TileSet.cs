using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace _2DRabbitPlatform.GFX
{
    public class TileSet
    {
        TileSetInfo info;
        Tile[] tiles;
        Texture2D texture;
        uint w, h;

        public TileSet(TileSetInfo info, Texture2D texture)
        {
            this.info = info;
            this.texture = texture;
            tiles = new Tile[info.Width * info.Height];
            w = info.Width; h = info.Height;
            clearSet();
        }

        public void clearSet()
        {
            for (uint y = 0; y < h; y++)
                for (uint x = 0; x < w; x++)
                    tiles[x + y * w] = new Tile(this, null, 0);
        }

        private void setTile(uint x, uint y, Tile t)
        {
            this.tiles[x + y * w] = t;
        }

        public static TileSet loadFromTexture(Texture2D texture, TileSetInfo info, TileMask[] masks)
        {
            TileSet ts = new TileSet(info, texture);

            for (uint y = 0; y < ts.Height; y++)
                for (uint x = 0; x < ts.Width; x++)
                {
                    ts.setTile(x, y, new Tile(ts, masks[x + y * ts.Width], x + y * ts.Width));
                }

            return ts;
        }

        public Texture2D Texture { get { return texture; } }

        public Tile this[int index] { get { return tiles[index]; } }
        public Tile this[int x, int y] { get { return tiles[x + y * w]; } }

        public uint Width { get { return info.Width; } }
        public uint Height { get { return info.Height; } }
        public TileSetInfo Info { get { return info; } }
    }
}
