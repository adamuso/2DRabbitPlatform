using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace _2DRabbitPlatform.Engine.Input
{
    public class KeyboardActionKey : ActionKey
    {
        Keys key;

        public KeyboardActionKey(Keys key) 
            : base()
        {
            this.key = key;
        }

        public override bool isKeyboardDown(KeyboardState ks)
        {
            return ks.IsKeyDown(key);
        }

        public override bool isKeyboardUp(KeyboardState ks)
        {
            return ks.IsKeyUp(key);
        }
    }
}
