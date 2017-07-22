using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.GFX.GUI.Forms
{
    public class Form : Control
    {
        int x, y, width, height;
        Color background, framecolor;
        SpriteFont font;
        Texture2D backgroudText, frameText;
        GUI gui;
        string title;
        Vector2 mouseOrigin;
        bool moveForm;

        public Form(GUI gui)
        {
            this.gui = gui;
            title = "Stworz nowy poziom";
            font = this.gui.Game.DefaultFont;
            background = new Color(127, 0, 0, 127);
            framecolor = background;
            framecolor.R -= 67;

            backgroudText = new Texture2D(gui.Game.GraphicsDevice, 1, 1);
            Color[] c = new Color[] { background };
            backgroudText.SetData<Color>(c);

            frameText = new Texture2D(gui.Game.GraphicsDevice, 1, 1);
            c = new Color[] { framecolor };
            frameText.SetData<Color>(c);

            x = 0;
            y = 0;
            width = 500;
            height = 300;

            controls = new ControlCollection(this);
            TextBox b = new TextBox(this);
            addControl(b);

            base.MouseUp += MouseUp;
            base.MouseDown += MouseDown;

            Initialize();
        }

        protected void MouseUp(object sender, Engine.Input.MouseEvent e)
        {
            if (moveForm)
            {
                moveForm = false;
            }
        }

        protected void MouseDown(object sender, Engine.Input.MouseEvent e)
        {
            if (!moveForm && new Rectangle(x, y, width, 25).Contains((int)e.Position.X, (int)e.Position.Y)) 
            {
                mouseOrigin = e.Position - new Vector2(x, y);
                moveForm = true;
            }
        }

        public void addControl(Control control)
        {
            controls.Add(control);
            control.Parent = this;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);
            sb.Draw(frameText, new Rectangle(x, y, width, height), Color.White);
            sb.Draw(backgroudText, ClientArea, Color.White);
            sb.Draw(backgroudText, new Rectangle(x + width - 15, y + 5, 10, 10), Color.Green);
            sb.DrawString(font, title, new Vector2(x + 2, y + 2), Color.White, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0);

            sb.End();

            for (int i = 0; i < controls.Count; i++)
                controls[i].Draw(sb);
        }

        public override void Update(GameTime gt)
        {
            for (int i = 0; i < controls.Count; i++)
                controls[i].Update(gt);

            if (moveForm && gui.InputManager.MouseManager.State.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                X = (int)(gui.InputManager.MouseManager.State.X - mouseOrigin.X);
                Y = (int)(gui.InputManager.MouseManager.State.Y - mouseOrigin.Y);
            }
        }

        public override GUI GUI { get { return gui; } }
        public override int X { get { return x; } set { x = value < 0 ? 0 : value; } }
        public override int Y { get { return y; } set { y = value < 0 ? 0 : value; } }
        public override int Width { get { return width; } }
        public override int Height { get { return height; } }
        public override Rectangle ClientArea { get { return new Rectangle(X + 5, Y + 20, Width - 10, Height - 25); } }
        public override SpriteFont Font { get { return font; } }
    }
}
