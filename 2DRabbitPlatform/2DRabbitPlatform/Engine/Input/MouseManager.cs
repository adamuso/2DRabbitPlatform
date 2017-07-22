using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine.Input
{
    public class MouseManager
    {
        MouseState ms;
        DoubleArray<MouseButton, bool> buttons;
        int wheelValue;

        public MouseManager()
        {
            buttons = new DoubleArray<MouseButton, bool>();
        }

        private bool isButtonDown(MouseButton key)
        {
            if (buttons.ContainsKey(key))
            {
                return buttons[key];
            }

            return false;
        }

        private bool isButtonUp(MouseButton key)
        {
            return !isButtonDown(key);
        }

        private void setKeyDown(MouseButton key)
        {
            if (key == MouseButton.NONE)
                return;

            buttons.Put(key, true);

            if (MouseDown != null)
                MouseDown.Invoke(this, new MouseEvent(this, key));
        }

        private void setKeyUp(MouseButton key)
        {
            if (key == MouseButton.NONE)
                return;

            buttons.Put(key, false);

            if (MouseUp != null)
                MouseUp.Invoke(this, new MouseEvent(this, key));
        }

        public void Update(GameTime gt)
        {
            ms = Mouse.GetState();
            List<MouseButton> keys = new List<MouseButton>(getPressedButtons(ms));

            foreach (MouseButton key in buttons.Keys)
            {
                if (isButtonDown(key))
                {
                    if (!keys.Contains(key))
                        setKeyUp(key);
                }
            }

            foreach (MouseButton key in keys)
            {
                if (isButtonUp(key))
                    setKeyDown(key);
            }

            if (wheelValue != ms.ScrollWheelValue)
                if (MouseWheel != null)
                    MouseWheel.Invoke(this, new MouseEvent(this, MouseButton.MIDDLE, (ms.ScrollWheelValue - wheelValue) / 120));

            wheelValue = ms.ScrollWheelValue;
        }

        public MouseEvent CreateEventOfActualState()
        {
            return new MouseEvent(this, getPressedButtons(State)[0], (ms.ScrollWheelValue - wheelValue) / 120);
        }

        private MouseButton[] getPressedButtons(MouseState ms)
        {
            List<MouseButton> buttons = new List<MouseButton>();
            if (ms.LeftButton == ButtonState.Pressed)
                buttons.Add(MouseButton.LEFT);
            if (ms.RightButton == ButtonState.Pressed)
                buttons.Add(MouseButton.RIGHT);
            if (ms.MiddleButton == ButtonState.Pressed)
                buttons.Add(MouseButton.MIDDLE);
            if (ms.XButton1 == ButtonState.Pressed)
                buttons.Add(MouseButton.X1);
            if (ms.XButton2 == ButtonState.Pressed)
                buttons.Add(MouseButton.X2);

            if (buttons.Count == 0)
                buttons.Add(MouseButton.NONE);

            return buttons.ToArray();
        }

        public event EventHandler<MouseEvent> MouseDown;
        public event EventHandler<MouseEvent> MouseUp;
        public event EventHandler<MouseEvent> MouseWheel;
        public MouseState State { get { return ms; } }
    }
}
