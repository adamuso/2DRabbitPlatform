using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine.Input
{
    public class InputManager
    {
        Dictionary<ActionKeyType, ActionKey> actionKeys;
        RabbitPlatform game;
        MouseManager mouseManager;
        KeyboardManager keyboardManager;

        public InputManager(RabbitPlatform instance, bool defaults)
        {
            this.game = instance;
            actionKeys = new Dictionary<ActionKeyType, ActionKey>();

            if (defaults)
            {
                actionKeys.Add(ActionKeyType.GO_LEFT, new KeyboardActionKey(Keys.Left));
                actionKeys.Add(ActionKeyType.GO_RIGHT, new KeyboardActionKey(Keys.Right));
                actionKeys.Add(ActionKeyType.JUMP, new KeyboardActionKey(Keys.LeftControl));
                actionKeys.Add(ActionKeyType.RUN, new KeyboardActionKey(Keys.LeftShift));
            }

            keyboardManager = new KeyboardManager();
            mouseManager = new MouseManager();
        }

        public void Update(GameTime gt)
        {
            mouseManager.Update(gt);
            keyboardManager.Update(gt);
        }

        public bool isDown(ActionKeyType type)
        {
            return actionKeys[type].isDown(mouseManager.State, keyboardManager.State);
        }

        public bool isUp(ActionKeyType type)
        {
            return actionKeys[type].isUp(mouseManager.State, keyboardManager.State);
        }

        public Location MouseLocation { get { return game.CurrentWorld.getLocation(mouseManager.State.X, mouseManager.State.Y); } }
        public Vector2 MousePosition { get { return new Vector2(mouseManager.State.X, mouseManager.State.Y); } }
        public KeyboardManager KeyboardManager { get { return keyboardManager; } }
        public MouseManager MouseManager { get { return mouseManager; } }
    }
}
