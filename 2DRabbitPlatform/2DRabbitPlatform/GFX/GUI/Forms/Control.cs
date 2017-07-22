using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2DRabbitPlatform.Engine.Input;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.GFX.GUI.Forms
{
    public abstract class Control
    {
        Control parent;
        bool focused;
        protected ControlCollection controls;
        private bool eventsInit;
        
        public Control()
        {
            focused = false;
            eventsInit = false;

            //KeyDown += onKeyDown;
            //KeyUp += onKeyUp;
            //MouseDown += onMouseDown;
            //MouseUp += onMouseUp;
            //WheelChange += onWheelChange;
            //MouseOver += onMouseOver;
        }

        public abstract void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb);
        public Control Parent { get { return parent; } set { this.parent = value; Initialize(); } }
        public abstract int X { get; set; }
        public abstract int Y { get; set;  }
        public abstract int Width { get; }
        public abstract int Height { get; }
        public bool Focused { get { return focused; } internal set { this.focused = value; } }
        public virtual GUI GUI { get { if (HasParent) return parent.GUI; else return null;  } }
        public abstract Microsoft.Xna.Framework.Rectangle ClientArea { get; }
        public abstract Microsoft.Xna.Framework.Graphics.SpriteFont Font { get; }
        public bool HasChilds { get { return controls != null; } }
        public bool HasParent { get { return parent != null; } }

        public virtual void Update(Microsoft.Xna.Framework.GameTime gt)
        {
            if (HasParent) if (parent.HasChilds)
            {
                if(parent.controls.FindTop(parent.GUI.InputManager.MousePosition) == this)
                {
                    onMouseOver(parent.GUI.InputManager.MouseManager, GUI.InputManager.MouseManager.CreateEventOfActualState());
                }
            }
        }

        public void Focus()
        {
            if (HasParent) if (parent.HasChilds)
                parent.controls.Focus(this);
        }

        protected virtual void Initialize() 
        {
            if (!eventsInit)
            {
                GUI.InputManager.KeyboardManager.KeyDown += onKeyDown;
                GUI.InputManager.KeyboardManager.KeyUp += onKeyUp;
                GUI.InputManager.MouseManager.MouseDown += onMouseDown;
                GUI.InputManager.MouseManager.MouseUp += onMouseUp;
                GUI.InputManager.MouseManager.MouseWheel += onWheelChange;

                eventsInit = true;
            }
        }

        protected virtual void onKeyDown(object state, KeyboardEvent e) 
        {
            if (HasParent)
            {
                if (parent.HasChilds)
                {
                    if (focused)
                    {
                        if (KeyDown != null)
                            KeyDown(state, e);
                    }
                }
            }
            else
            {
                if (focused)
                    if (KeyDown != null)
                        KeyDown(state, e);
            }
        }

        protected virtual void onKeyUp(object state, KeyboardEvent e) 
        {
            if (HasParent)
            {
                if (parent.HasChilds)
                {
                    if (focused)
                    {
                        if (KeyUp != null)
                            KeyUp(state, e);
                    }
                }
            }
            else
            {
                if (focused)
                    if (KeyUp != null)
                        KeyUp(state, e);
            }
        }

        protected virtual void onMouseDown(object state, MouseEvent e)
        {
            if (HasParent)
            {
                if (parent.HasChilds)
                {
                    if (parent.controls.FindTop(e.Position) == this)
                    {
                        if (MouseDown != null)
                            MouseDown(state, e);

                        Focus();
                    }
                }
            }
            else
            {
                Rectangle controlArea = new Rectangle(X, Y, Width, Height);

                if (controlArea.Contains(new Point((int)e.Position.X, (int)e.Position.Y)))
                    if (MouseDown != null)
                        MouseDown(state, e);
            }
        }

        protected virtual void onMouseUp(object state, MouseEvent e) 
        {
            if (HasParent) 
            { 
                if (parent.HasChilds)
                {
                    if (parent.controls.FindTop(e.Position) == this)
                    {
                        if(MouseUp != null)
                            MouseUp(state, e);
                    }
                } 
            }
            else
            {
                Rectangle controlArea = new Rectangle(X, Y, Width, Height);

                if(controlArea.Contains(new Point((int)e.Position.X, (int)e.Position.Y)))
                    if (MouseUp != null)
                        MouseUp(state, e);
            }
        }

        protected virtual void onWheelChange(object state, MouseEvent e) 
        {
            if (HasParent)
            {
                if (parent.HasChilds)
                {
                    if (parent.controls.FindTop(e.Position) == this)
                    {
                        if (WheelChange != null)
                            WheelChange(state, e);
                    }
                }
            }
            else
            {
                Rectangle controlArea = new Rectangle(X, Y, Width, Height);

                if (controlArea.Contains(new Point((int)e.Position.X, (int)e.Position.Y)))
                    if (WheelChange != null)
                        WheelChange(state, e);
            }
        }
        
        protected virtual void onMouseOver(object state, MouseEvent e) 
        {
            if (HasParent)
            {
                if (parent.HasChilds)
                {
                    if (parent.controls.FindTop(e.Position) == this)
                    {
                        if (MouseOver != null)
                            MouseOver(state, e);
                    }
                }
            }
            else
            {
                Rectangle controlArea = new Rectangle(X, Y, Width, Height);

                if (controlArea.Contains(new Point((int)e.Position.X, (int)e.Position.Y)))
                    if (MouseOver != null)
                        MouseOver(state, e);
            }
        }

        public event EventHandler<KeyboardEvent> KeyDown;
        public event EventHandler<KeyboardEvent> KeyUp;
        public event EventHandler<MouseEvent> MouseDown;
        public event EventHandler<MouseEvent> MouseUp;
        public event EventHandler<MouseEvent> WheelChange;
        public event EventHandler<MouseEvent> MouseOver;
    }
}
