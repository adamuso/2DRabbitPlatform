using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DRabbitPlatform.GFX.GUI.Forms
{
    public class Button : Control
    {
        int x, y, width, height;
        Control parent;
        Color backgroundColor, frameColor;
        Texture2D backText, frameText;
        string text;

        public Button(Control parent)
        {
            x = 50;
            y = 50;
            width = 75;
            height = 25;
            this.parent = parent;
            this.text = "Hello!";

            backgroundColor = new Color(0, 0, 127, 127);
            frameColor = new Color(0, 0, 100, 127);

            backText = new Texture2D(GUI.Game.GraphicsDevice, 1, 1);
            backText.SetData<Color>(new Color[] { backgroundColor });

            frameText = new Texture2D(GUI.Game.GraphicsDevice, 1, 1);
            frameText.SetData<Color>(new Color[] { frameColor });
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.Draw(frameText, new Rectangle(X, Y, width, height), Color.White);

            if (new Rectangle(X, Y, width, height).Contains((int)GUI.InputManager.MousePosition.X, (int)GUI.InputManager.MousePosition.Y))
                if(GUI.InputManager.MouseManager.State.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    sb.Draw(backText, ClientArea, Color.Gray);  
                else
                    sb.Draw(backText, ClientArea, Color.LightGray);
            else
                sb.Draw(backText, ClientArea, Color.White);

            sb.DrawString(parent.Font, text, Utilities.centerText(ClientArea, Font.MeasureString(text), 0.6f), Color.White, 0, Vector2.Zero, 0.6f, SpriteEffects.None, 0);

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
        
        }

        public override int X
        {
            get { return parent.X + x; }
            set { x = value; }
        }

        public override int Y
        {
            get { return parent.Y + y; }
            set { y = value; }
        }

        public override int Width
        {
            get { return width; }
        }

        public override int Height
        {
            get { return height; }
        }

        public override Rectangle ClientArea { get { return new Rectangle(X + 3, Y + 3, width - 6, height - 6); } }
        public override SpriteFont Font { get { return parent.Font; } }
        public string Text { get { return text; } set { text = value; } }
    }
}
