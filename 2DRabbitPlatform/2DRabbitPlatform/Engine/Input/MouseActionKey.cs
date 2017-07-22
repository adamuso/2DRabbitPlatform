using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace _2DRabbitPlatform.Engine.Input
{
    public class MouseActionKey : ActionKey
    {
        MouseButton button;

        public MouseActionKey(MouseButton button) 
            : base()
        {
            this.button = button;
        }

        public override bool isMouseDown(MouseState ms)
        {
            if (button == MouseButton.LEFT)
                return ms.LeftButton == ButtonState.Pressed;
            else if (button == MouseButton.RIGHT)
                return ms.LeftButton == ButtonState.Pressed;
            else if (button == MouseButton.MIDDLE)
                return ms.MiddleButton == ButtonState.Pressed;
            else if (button == MouseButton.X1)
                return ms.XButton1 == ButtonState.Pressed;
            else if (button == MouseButton.X2)
                return ms.XButton2 == ButtonState.Pressed;

            return false;
        }

        public override bool isMouseUp(MouseState ms)
        {
            if (button == MouseButton.LEFT)
                return ms.LeftButton == ButtonState.Released;
            else if (button == MouseButton.RIGHT)
                return ms.LeftButton == ButtonState.Released;
            else if (button == MouseButton.MIDDLE)
                return ms.MiddleButton == ButtonState.Released;
            else if (button == MouseButton.X1)
                return ms.XButton1 == ButtonState.Released;
            else if (button == MouseButton.X2)
                return ms.XButton2 == ButtonState.Released;

            return false;
        }
    }
}
