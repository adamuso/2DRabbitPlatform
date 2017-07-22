using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _2DRabbitPlatform.Engine.Input
{
    public class KeyboardManager
    {
        KeyboardState ks;
        DoubleArray<Keys, bool> keydictionary;

        public KeyboardManager()
        {
            keydictionary = new DoubleArray<Keys, bool>();
        }

        private bool isKeyDown(Keys key)
        {
            if (keydictionary.ContainsKey(key))
            {
                return keydictionary[key];
            }

            return false;
        }

        private bool isKeyUp(Keys key)
        {
            return !isKeyDown(key);
        }

        private void setKeyDown(Keys key)
        {
            keydictionary.Put(key, true);

            if(KeyDown != null)
                KeyDown.Invoke(this, new KeyboardEvent(this, key));
        }

        private void setKeyUp(Keys key)
        {
            keydictionary.Put(key, false);

            if (KeyUp != null)
                KeyUp.Invoke(this, new KeyboardEvent(this, key));
        }

        public void Update(GameTime gt)
        {
            ks = Keyboard.GetState();
            List<Keys> keys = new List<Keys>(ks.GetPressedKeys());

            foreach (Keys key in keydictionary.Keys)
            {
                if (isKeyDown(key))
                {
                    if (!keys.Contains(key))
                        setKeyUp(key);
                }
            }

            foreach (Keys key in keys)
            {
                if (isKeyUp(key))
                    setKeyDown(key);
            }
        }

        public event EventHandler<KeyboardEvent> KeyDown;
        public event EventHandler<KeyboardEvent> KeyUp; 
        public KeyboardState State { get { return ks; } }
    }
}
