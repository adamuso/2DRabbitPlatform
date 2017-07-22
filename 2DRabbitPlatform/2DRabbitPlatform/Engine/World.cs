using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2DRabbitPlatform.GFX;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using _2DRabbitPlatform.Engine.Input;

namespace _2DRabbitPlatform.Engine
{
    public class World
    {
        //protected TileMap[] map;
        RabbitPlatform game;
        Camera2D worldCamera;
        Entity.Player player;

        Entity.EntityManager entityManager;
        CollisionManager collisionManager;
       
        PlayerController playerController;
        Random random;
        Level level;

        bool enabled;

        public World(RabbitPlatform instance)
        {
            this.game = instance;
            this.worldCamera = new Camera2D(game);
            this.random = new Random();

            this.entityManager = new Entity.EntityManager(this);
            this.collisionManager = new CollisionManager(this);

            this.playerController = new PlayerController(this);
            
            this.enabled = false;
        }

        public void prepareWorld()
        {
            //tileManager.createEvTile(0, 12, 3, false, new Event.SpawnEvent() { EntityID = (int)Entity.StandardEntities.ITEM_CARROT });
            Level.prepareLevel();
            entityManager.clearEntities();
            player = playerController.createPlayer();
            this.worldCamera.setLimit(0, 0, Level.Map.Width * Tile.STANDARD_GTILE_WIDTH - (int)game.Resolution.Scene.X, Level.Map.Height * Tile.STANDARD_GTILE_HEIGHT - (int)game.Resolution.Scene.Y);
            this.worldCamera.followEntity(player);
        }
#if DEBUG
        public void createWorld(ushort layers, ushort mainLayer)
        {
            Level level = new Level(this, layers, mainLayer);

            level.RenderManager.getEntityLayer().Map = TileMap.loadFromStream(this.game, new System.IO.FileStream("TileMap.txt", System.IO.FileMode.Open));

            MapLayer layer = level.RenderManager.getLayer(level.RenderManager.Layers.Count - 2);

            layer.Map = new TileMap(8, 8, level.RenderManager.getEntityLayer().Map.TileSet);
            layer.Map.setTile(1, 1, new TileData(9));
            layer.Map.setTile(6, 4, new TileData(9));
            layer.Map.setTile(4, 3, new TileData(9));
            layer.Map.setTile(2, 2, new TileData(9));
            layer.Map.setTile(2, 7, new TileData(9));
            layer.Map.setTile(7, 5, new TileData(9));
            layer.AutoXScroll = -6f;
            layer.AutoYScroll = 2f;
            layer.YSpeed = 0f;
            layer.XSpeed = 2f;
            layer.WidthWrap = true;
            layer.HeightWrap = true;

            layer = level.RenderManager.getLayer(0);

            layer.Map = new TileMap(8, 8, level.RenderManager.getEntityLayer().Map.TileSet);

            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    layer.Map.setTile(x, y, new TileData(10 + y * 10 + x));

            layer.WidthWrap = true;
            layer.HeightWrap = true;

            level.TileManager.createEvTile(0, 2, 2, false, new Event.PlayerSpawnEvent());
            level.TileManager.createEvTile(1, 16, 5, false, new Event.OneWayEvent());//new Event.WarpEvent(25, 2));
            level.TileManager.createEvTile(0, 15, 6, false, new Event.WarpEvent() { X = 25, Y = 2 });
            level.TileManager.createEvTile(0, 7, 5, false, new Event.StopAIEvent());
            level.TileManager.createEvTile(0, 19, 4, false, new Event.StopAIEvent());
            level.TileManager.createATile(new int[] { 1, 2, 3, 4, 5 }, 19, 3, false, 100, true, new Event.OneWayEvent());
            level.TileManager.createEvTile(0, 6, 0, false, new Event.SpawnEvent() { EntityID = (int)Entity.StandardEntities.WOLF });

            level.TileManager.initializeEvents();

            entityManager.clearEntities();
            player = playerController.createPlayer();
            player.setLocation(level.SpawnPoint);
            //entityManager.createEntity(Entity.StandardEntities.WOLF).setLocation(new Location(this, 192, 0));
            entityManager.createEntity(Entity.StandardEntities.ITEM_CARROT).setLocation(new Location(this, 32, 32));

            this.worldCamera.setLimit(0, 0, Level.Map.Width * Tile.STANDARD_GTILE_WIDTH - (int)game.Resolution.Scene.X, Level.Map.Height * Tile.STANDARD_GTILE_HEIGHT - (int)game.Resolution.Scene.Y);
            this.worldCamera.followEntity(player);
        }
#endif
        public void refreshCamera()
        {
            this.worldCamera.setLimit(0, 0, Level.Map.Width * Tile.STANDARD_GTILE_WIDTH - (int)game.Resolution.Scene.X, Level.Map.Height * Tile.STANDARD_GTILE_HEIGHT - (int)game.Resolution.Scene.Y);
        }

        public void changeLevel(Level level)
        {
            this.level = level;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            if(level != null)
                level.Draw(sb);
        }

        public virtual void Draw(SpriteBatch sb, int layer)
        {
            if(layer == level.EntityLayerId)
                entityManager.DrawEntities(sb);
        }

        public virtual void Update(GameTime gt)
        {
            if (enabled)
            {
                worldCamera.Update(gt);
                Level.Update(gt);
                playerController.Update(gt);
                entityManager.UpdateEntities(gt);
            }
        }

        public bool interact(Entity.Action.ActionArgs action)
        {
            return false;
        }

        public Location getStaticPosition(int x, int y)
        {
            return getStaticPosition(new Vector2(x, y));
        }

        public Location getStaticPosition(Vector2 pos)
        {
            return new Location(this, Vector2.Transform(pos, Matrix.Invert(worldCamera.Projection))); //* Game.Resolution.Projection)));
        }

        public Location getAppToWorld(int x, int y)
        {
            return getAppToWorld(new Vector2(x, y));
        }

        public Location getAppToWorld(Vector2 pos)
        {
            return new Location(this, Vector2.Transform(pos, Matrix.Invert(worldCamera.Projection * Game.Resolution.Projection)));
        }

        //public Location transformToApplication(int x, int y)
        //{
        //    return transformToApplication(new Vector2(x, y));
        //}

        //public Location transformToApplication(Vector2 pos)
        //{
        //    return new Location(this, Vector2.Transform(pos, worldCamera.Projection * Game.Resolution.Projection));
        //}

        public Location getLocation(int x, int y)
        {
            return getLocation(new Vector2(x, y));
        }

        public Location getLocation(Vector2 vector)
        {
            return new Location(this, vector);
        }

        public Entity.Player Player { get { return player; } }
        public RabbitPlatform Game { get { return game; } }
        public Camera2D WorldCamera { get { return worldCamera; } }
        public CollisionManager CollisionManager { get { return collisionManager; } }
        public InputManager InputManager { get { return game.InputManager; } }
        public Entity.EntityManager EntityManager { get { return entityManager; } }
        public Random Random { get { return random; } }
        public Level Level { get { return level; } }
        public bool Enabled { get { return enabled; } set { enabled = value; } }
        public RenderManager RenderManager { get { return level.RenderManager; } }
    }
}
