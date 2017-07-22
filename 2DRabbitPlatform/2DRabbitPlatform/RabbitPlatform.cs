using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using _2DRabbitPlatform.Engine;
using _2DRabbitPlatform.GFX;
using _2DRabbitPlatform.Engine.Input;
using _2DRabbitPlatform.GFX.GUI;

namespace _2DRabbitPlatform
{
    public class RabbitPlatform : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Resolution resolution;
        TextureManager textureManager;
        InputManager inputManager;
        World currentWorld;
        GUI gui;
        SpriteFont defaultFont;
        TextureLoader textureLoader;
        Viewport defaultViewport;

        public RabbitPlatform()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            textureManager = new TextureManager(this);
            resolution = new Resolution(this);

            resolution.setResolution(1280, 720);
            resolution.setSceneSize(800, 600);
        }

        protected override void Initialize()
        {
            base.Initialize();
            base.IsMouseVisible = true;

            this.textureLoader = new TextureLoader(GraphicsDevice);

            this.inputManager = new InputManager(this, true);

            createTileSet(32, "TS.png", "TS_mask.png", "TS.gts");

            currentWorld = new Engine.Editor.EditorWorld(this);
            currentWorld.changeLevel(loadLevel("TestWorld.glv"));
            currentWorld.prepareWorld();
            currentWorld.Enabled = true;

            //currentWorld = new Engine.Editor.EditorWorld(this);
            //currentWorld.prepareWorld();
            //currentWorld.createWorld(8, 4);
            saveLevel("TestWorld.glv", "TS.gts");

            gui = new GUI(this);

            base.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 15);

            //font

            defaultViewport = GraphicsDevice.Viewport;
        }

        public Level loadLevel(string levelName)
        {
            return Engine.Files.LevelFile.loadLevel(this, levelName, true);
        }

        public void createTileSet(byte baseTileSize, string graphicsPart, string maskPart, string outputFile)
        {
            Engine.Files.TileSetFile.generate(this, baseTileSize, graphicsPart, maskPart, outputFile);
        }

        public void saveLevel(string levelFileName, string usedTileSetFile)
        {
            Engine.Files.LevelFile.saveLevel(currentWorld.Level, levelFileName, usedTileSetFile);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureManager.loadTextures();
            defaultFont = Content.Load<SpriteFont>("DefaultFont");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            this.inputManager.Update(gameTime);

            this.gui.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //if (GraphicsDevice.Viewport.Height != defaultViewport.Height || GraphicsDevice.Viewport.Width != defaultViewport.Width)
              //  GraphicsDevice.Viewport = defaultViewport;

            GraphicsDevice.Clear(new Color(20, 20, 80));

            this.gui.Draw(spriteBatch);
        }

        public float getTimes(GameTime time)
        {
            
            Console.WriteLine("TIME: " + time.ElapsedGameTime.TotalMilliseconds);
            return (float)(time.ElapsedGameTime.TotalMilliseconds / 15);
        }

        public TextureManager TextureManager { get { return textureManager; } }
        public InputManager InputManager { get { return inputManager; } }
        public World CurrentWorld { get { return currentWorld; } }
        public GraphicsDeviceManager GraphicsManager { get { return graphics; } }
        public Resolution Resolution { get { return resolution; } }
        public SpriteFont DefaultFont { get { return defaultFont; } }
        public TextureLoader TextureLoader { get { return textureLoader; } }
    }
}
