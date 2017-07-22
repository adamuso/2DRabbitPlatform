using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.GFX.GUI.Forms
{
    public class TextBox : Control
    {
        private static RasterizerState _rasterizerScissorsState = new RasterizerState() { ScissorTestEnable = true };

        int x, y, width, height;
        Color backgroundColor, frameColor, topColor;
        Texture2D backText, frameText;
        string text, drawText;
        bool scrolling;
        int text_y_scroll;

        public TextBox(Control parent)
        {
            x = 50;
            y = 50;
            width = 300;
            height = 200;
            Parent = parent;
            this.Text = "";//"Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello Hello ";
            scrolling = true;

            backgroundColor = new Color(220, 220, 255, 255);
            frameColor = new Color(0, 0, 100, 127);
            topColor = Color.White;

            backText = new Texture2D(GUI.Game.GraphicsDevice, 1, 1);
            backText.SetData<Color>(new Color[] { backgroundColor });

            frameText = new Texture2D(GUI.Game.GraphicsDevice, 1, 1);
            frameText.SetData<Color>(new Color[] { frameColor });

            base.KeyDown += _KeyDown;
            base.WheelChange += _WheelChange;
            base.MouseOver += _MouseOver;
        }

        void _WheelChange(object sender, Engine.Input.MouseEvent e)
        {
            text_y_scroll += e.WheelDelta * 5;

            if (text_y_scroll < 0)
                text_y_scroll = 0;
        }

        void _KeyDown(object sender, Engine.Input.KeyboardEvent e)
        {
            if (e.CanDecode())
            {
                char k = e.DecodeKey();
                Text += k;
            }
            else
            {
                switch (e.Key)
                {
                    case Microsoft.Xna.Framework.Input.Keys.Enter:
                        Text += "\r\n";
                        break;
                    case Microsoft.Xna.Framework.Input.Keys.Back:
                        Text = Text.Substring(0, Text.Length - 1);
                        break;
                }
            }
        }

        void _MouseOver(object sender, Engine.Input.MouseEvent e)
        {
            topColor = Color.LightGray;

            if (e.Button == Engine.Input.MouseButton.LEFT)
                topColor = Color.Gray;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, _rasterizerScissorsState);

            sb.Draw(frameText, new Rectangle(X, Y, width, height), Color.White);

            //if (new Rectangle(X, Y, width, height).Contains((int)GUI.InputManager.MousePosition.X, (int)GUI.InputManager.MousePosition.Y))
            //    if(GUI.InputManager.MouseManager.State.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            //        sb.Draw(backText, ClientArea, Color.Gray);  
            //    else
            //        sb.Draw(backText, ClientArea, Color.LightGray);
            //else
                
            sb.Draw(backText, ClientArea, topColor);
            topColor = Color.White;

            Rectangle oldsc = sb.GraphicsDevice.ScissorRectangle;

            sb.GraphicsDevice.ScissorRectangle = ClientArea;

            sb.DrawString(Parent.Font, drawText, new Vector2(ClientArea.X + 3, ClientArea.Y + 3 - text_y_scroll), Color.Black); //, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //sb.DrawString(font, drawString, new Vector2(destination.X, destination.Y - yoffset), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);

            sb.GraphicsDevice.ScissorRectangle = oldsc;

            sb.End();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
            base.Update(gt);
        }

        public override int X
        {
            get { return Parent.X + x; }
            set { x = value; }
        }

        public override int Y
        {
            get { return Parent.Y + y; }
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
        public override SpriteFont Font { get { return Parent.Font; } }

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                string old = text;
                text = value;
                string newtext = GFX.GUI.Renderer.TextRenderer.parseText(text, Font, ClientArea, 1f);
                if (!GFX.GUI.Renderer.TextRenderer.isOverHeight(newtext, Font, ClientArea, 1f) || scrolling)
                    drawText = newtext;
                else
                    text = old;
            }
        }
    }
}
