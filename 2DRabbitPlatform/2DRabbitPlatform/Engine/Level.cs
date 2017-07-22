using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using _2DRabbitPlatform.GFX;

namespace _2DRabbitPlatform.Engine
{
    public class Level
    {
        World world;
        TileManager tileManager;
        List<Location> spawnPoints;
        RenderManager renderManager;

        public Level(World world, ushort layers, ushort mainlayer)
        {
            this.world = world;
            this.tileManager = new TileManager(this);
            this.spawnPoints = new List<Location>();
            this.renderManager = new RenderManager(world, layers, mainlayer);
        }

        public void Update(GameTime gt)
        {
            renderManager.Update(gt);
            tileManager.Update(gt);
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < renderManager.Layers.Count; i++)
            {
                renderManager.renderMap(i);
                world.Draw(sb, i);
            }
        }

        public void initializeLayer(TileMapInfo map, int layer)
        {
            MapLayer maplay = renderManager.getLayer(layer);
            maplay.Map = map.Map;
            maplay.AutoScrollEnabled = true;
            maplay.XSpeed = map.XScrollSpeed;
            maplay.YSpeed = map.YScrollSpeed;
            maplay.WidthWrap = map.WidthWrap;
            maplay.HeightWrap = map.HeightWrap;
            maplay.AutoXScroll = map.AutoXSpeed;
            maplay.AutoYScroll = map.AutoYSpeed;
            tileManager.addDynamicTiles(map.EventTiles);
        }

        public void prepareLevel()
        {
            tileManager.initializeEvents();
        }

        public void addSpawnPoint(Location l)
        {
            spawnPoints.Add(l);
        }

        public TileData getTile(Location location)
        {
            return renderManager.getEntityLayer().Map.getTile(location.getTileX(), location.getTileY());
        }

        public TileData getTile(int x, int y)
        {
            return renderManager.getEntityLayer().Map.getTile(x, y);
        }

        private Location getSpawnPoint()
        {
            if (spawnPoints.Count > 1)
            {
                return spawnPoints[(int)(world.Random.NextDouble() * spawnPoints.Count)];
            }
            else
                return spawnPoints[0];
        }

        public RenderManager RenderManager { get { return renderManager; } }
        public TileManager TileManager { get { return tileManager; } }
        public TileMap Map { get { return renderManager.getEntityLayer().Map; } }
        public TileSet TileSet { get { return Map.TileSet; } }
        public int EntityLayerId { get { return renderManager.EntityLayerID; } }
        public Location SpawnPoint { get { return getSpawnPoint(); } }
        public World World { get { return world; } }
    }
}
