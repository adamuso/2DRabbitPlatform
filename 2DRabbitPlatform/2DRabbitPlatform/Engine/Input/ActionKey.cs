using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace _2DRabbitPlatform.Engine.Input
{
    public abstract class ActionKey
    {
        protected ActionKey()
        {

        }

        public virtual bool isMouseDown(MouseState ms) { return false; }
        public virtual bool isKeyboardDown(KeyboardState ks) { return false; }
        public virtual bool isMouseUp(MouseState ms) { return false; }
        public virtual bool isKeyboardUp(KeyboardState ks) { return false; }

        public bool isDown(MouseState ms, KeyboardState ks)
        {
            return isMouseDown(ms) || isKeyboardDown(ks);
        }

        public bool isUp(MouseState ms, KeyboardState ks)
        {
            return isMouseUp(ms) || isKeyboardUp(ks);
        }
    }
}
