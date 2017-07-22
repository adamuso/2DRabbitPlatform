using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DRabbitPlatform.GFX.GUI
{
    public class GUI
    {
        readonly Rectangle[] guiareas = new Rectangle[] { new Rectangle(320, 620, 640, 100) };

        Forms.Form f;
        Screen currentScreen;
        RabbitPlatform game;
        Screen gameScreen;

        public GUI(RabbitPlatform instance)
        {
            this.game = instance;
            this.gameScreen = new GameScreen(this);//new GameScreen(this);
            this.currentScreen = gameScreen;
            //f = new Forms.Form(this);
        }

        public void Draw(SpriteBatch sb)
        {      
            currentScreen.Draw(sb);
            //f.Draw(sb);
        }

        public void Update(GameTime gt)
        {
            currentScreen.Update(gt);
            //f.Update(gt);
        }

        public Point toPoint(Vector2 vec)
        {
            return new Point((int)vec.X, (int)vec.Y);
        }

        public Vector2 toVector(Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public RabbitPlatform Game { get { return game; } }
        public Engine.Input.InputManager InputManager { get { return game.InputManager; } }
    }
}
