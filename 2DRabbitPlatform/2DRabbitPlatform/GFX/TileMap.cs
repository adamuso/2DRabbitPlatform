using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2DRabbitPlatform.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace _2DRabbitPlatform.GFX
{
    public class TileMap
    {
        TileData[,] mapTiles;
        TileSet tileSet;
        int w, h;

        public TileMap(int width, int height, TileSet set)
        {
            this.mapTiles = new TileData[width, height];
            this.tileSet = set;
            this.w = width;
            this.h = height;
            clearMap();
        }

        public void clearMap()
        {
            for(int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    mapTiles[x, y] = new TileData();
                }
        }

        public void Draw(MapLayer layer)
        {
            this.Draw(layer, 0, 0);

            //Rectangle onScreen = getScreenMapPart(layer, 0, 0);

            //for(int x = onScreen.Left; x < onScreen.Right; x++)
            //    for(int y = onScreen.Top; y < onScreen.Bottom; y++)
            //    {
            //        if (!getTile(x, y).isEmpty)
            //            sb.Draw(tileSet.Texture, new Rectangle(x * tileSet[mapTiles[x, y].ID].Width, y * tileSet[mapTiles[x, y].ID].Height, tileSet[mapTiles[x, y].ID].Width, tileSet[mapTiles[x, y].ID].Height), tileSet[mapTiles[x, y].ID].Source, Color.White, 0f, Vector2.Zero, mapTiles[x,y].Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f); 
            //            //sb.Draw(ts, new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(mapTiles[x, y].ID * 32, 0, 32, 32), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            //    }
        }
        public void Draw(MapLayer layer, int mapx, int mapy)
        {
            Rectangle onScreen = getScreenMapPart(layer, mapx, mapy);

            for (int x = onScreen.Left; x < onScreen.Right; x++)
                for (int y = onScreen.Top; y < onScreen.Bottom; y++)
                {
                    if (!getTile(x, y).isEmpty)
                        layer.Draw(tileSet.Texture, new Rectangle(mapx * w * Tile.STANDARD_GTILE_WIDTH + x * tileSet[mapTiles[x, y].ID].Width, mapy * h * Tile.STANDARD_GTILE_HEIGHT + y * tileSet[mapTiles[x, y].ID].Height, tileSet[mapTiles[x, y].ID].Width, tileSet[mapTiles[x, y].ID].Height), tileSet[mapTiles[x, y].ID].Source, Color.White, 0f, Vector2.Zero, mapTiles[x, y].Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);

                    if (getTile(x, y).HasEvents)
                    {
                        Engine.Event.EventTile e = (Engine.Event.EventTile)getTile(x, y);
                        layer.Draw(tileSet.Texture, new Rectangle(mapx * w * Tile.STANDARD_GTILE_WIDTH + x * tileSet[mapTiles[x, y].ID].Width, mapy * h * Tile.STANDARD_GTILE_HEIGHT + y * tileSet[mapTiles[x, y].ID].Height, tileSet[mapTiles[x, y].ID].Width, tileSet[mapTiles[x, y].ID].Height), tileSet[mapTiles[x, y].ID].Source, new Color(128, 128, 128, 128), 0f, Vector2.Zero, mapTiles[x, y].Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.95f);
                        layer.DrawString(layer.World.Game.DefaultFont, e.Events[0].DisplayName, new Vector2(mapx * w * Tile.STANDARD_GTILE_WIDTH + x * tileSet[mapTiles[x, y].ID].Width, mapy * h * Tile.STANDARD_GTILE_HEIGHT + y * tileSet[mapTiles[x, y].ID].Height), Color.Gray, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.9f);
                    }  
 
                    //sb.Draw(ts, new Rectangle(x * 32, y * 32, 32, 32), new Rectangle(mapTiles[x, y].ID * 32, 0, 32, 32), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
                }
        }

        public static TileMap loadFromStream(RabbitPlatform game, Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);

            string tileset = Utility.readToBrakpoint(reader);
            tileset = tileset.Substring(1);
            int width = int.Parse(Utility.readToBrakpoint(reader));
            int height = int.Parse(Utility.readToBrakpoint(reader));

            TileMap map = new TileMap(width, height, Engine.Files.TileSetFile.loadTileSet(game, tileset));//TileSet.loadFromTexture(TextureManager.Instance.groundTileSet, TileSetInfo.loadFromStream(new FileStream("TSInfo.txt", FileMode.Open))));

            for(int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    map.setTile(x, y, new TileData(int.Parse(Utility.readToBrakpoint(reader))));
                }

            return map;
        }

        private Rectangle getScreenMapPart(MapLayer layer, int xoffset, int yoffset)
        {
            Rectangle part = layer.overlapWithScreen(new Rectangle(xoffset * w * Tile.STANDARD_GTILE_WIDTH, yoffset * h * Tile.STANDARD_GTILE_HEIGHT, w * Tile.STANDARD_GTILE_WIDTH, h * Tile.STANDARD_GTILE_HEIGHT));
            Rectangle buffer = new Rectangle();
            buffer.X = part.X / 32;
            buffer.Y = part.Y / 32;
            buffer.Width = (int)Math.Ceiling(part.Width / 32f) + 1;
            buffer.Height = (int)Math.Ceiling(part.Height / 32f) + 1;

            return buffer;
        }

        public TileData getTile(int x, int y)
        {
            if(x >= 0 && y >= 0 && x < w && y < h)
                return mapTiles[x, y];

            return TileData.Empty;
        }
        public TileData getTile(float x, float y)
        {
            return getTile((int)(x / Tile.STANDARD_GTILE_WIDTH), (int)(y / Tile.STANDARD_GTILE_HEIGHT));
        }
        public TileData getTile(Vector2 pos)
        {
            return getTile(pos.X, pos.Y);
        }

        public void setTile(int x, int y, TileData tile)
        {
            if (x >= 0 && y >= 0 && x < w && y < h)
                mapTiles[x, y] = tile;
        }
        public void setTile(float x, float y, TileData tile)
        {
            setTile((int)(x / Tile.STANDARD_GTILE_WIDTH), (int)(y / Tile.STANDARD_GTILE_HEIGHT), tile);
        }
        public void setTile(Vector2 pos, TileData tile)
        {
            setTile(pos.X, pos.Y, tile);
        }

        public TileSet TileSet { get { return tileSet; } }

        public int Width { get { return w; } }
        public int Height { get { return h; } }
    }
}
