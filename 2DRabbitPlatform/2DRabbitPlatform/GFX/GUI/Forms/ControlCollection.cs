using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DRabbitPlatform.GFX.GUI.Forms
{
    public class ControlCollection : List<Control>
    {
        Control parent;
        Control focused;

        public ControlCollection(Control parent) :
            base()
        {
            this.parent = parent;
        }

        public bool IsOnTop(Control control)
        {
            if (control == parent)
                if (Count == 0)
                    return true;
                else
                    return false;

            Rectangle controlArea = new Rectangle(control.X, control.Y, control.Width, control.Height);

            int position = -1;

            for (int i = 0; i < Count; i++)
            {
                if (this[i] == control)
                    position = i;

                if (position != -1)
                {
                    Rectangle higherArea = new Rectangle(this[i].X, this[i].Y, this[i].Width, this[i].Height);

                    if (controlArea.Intersects(higherArea))
                        return false;
                }
            }

            return true;
        }

        public void Focus(Control control)
        {
            if (control == parent)
            {
                control = parent;
            }

            if(focused != null)
                focused.Focused = false;
            
            focused = control;
            focused.Focused = true;
        }

        public Control FindFocused()
        {
            return focused;
        }

        public Control FindTop(Vector2 point)
        {
            Control top = parent;

            for (int i = 0; i < Count; i++)
            {
                Rectangle higherArea = new Rectangle(this[i].X, this[i].Y, this[i].Width, this[i].Height);

                if (higherArea.Contains(new Point((int)point.X, (int)point.Y)))
                    top = this[i];
            }

            return top;
        }

        public new void Add(Control control)
        {
            if (base.Contains(control))
                throw new Exception("Dulpicate items in collection");
            else
            {
                base.Add(control);
                control.Parent = parent;
            }
        }

        public void AddRange(params Control[] controls)
        {
            foreach (Control cnt in controls)
                Add(cnt);
        }

        public new void Insert(int index, Control control)
        {
            if (base.Contains(control))
                throw new Exception("Dulpicate items in collection");
            else
            {
                base.Insert(index, control);
                control.Parent = parent;
            }
        }

        public void InsertRange(int index, params Control[] controls)
        {
            foreach (Control cnt in controls)
            {
                if (base.Contains(cnt))
                    throw new Exception("Dulpicate items in collection");

                cnt.Parent = parent;
            }

            base.InsertRange(index, controls);
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < Count; i++)
                this[i].Draw(sb);
        }

        public void Update(GameTime gt)
        {
            for (int i = 0; i < Count; i++)
                this[i].Update(gt);
        }
    }
}
