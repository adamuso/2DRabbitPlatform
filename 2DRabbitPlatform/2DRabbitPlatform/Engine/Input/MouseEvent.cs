using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine.Input
{
    public class MouseEvent : EventArgs
    {
        private MouseManager mouseManager;
        private MouseButton button;
        private int wheelDelta;

        public MouseEvent(MouseManager mouseManager, MouseButton button, int wheelDelta = 0)
        {
            this.mouseManager = mouseManager;
            this.button = button;
            this.wheelDelta = wheelDelta;
        }

        public Vector2 Position { get { return new Vector2(mouseManager.State.X, mouseManager.State.Y); } }
        public int WheelDelta { get { return wheelDelta; } }
        public MouseButton Button { get { return button; } }
        public MouseManager MouseManager { get { return mouseManager; } }
    }
}
