using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace _2DRabbitPlatform.Engine.Input
{
    public class KeyboardEvent : EventArgs
    {
        Keys key;
        KeyboardManager manager;
        bool shift, ctrl, alt;

        public KeyboardEvent(KeyboardManager ks, Keys key) : base()
        {
            this.manager = ks;
            shift = ks.State.IsKeyDown(Keys.LeftShift) || ks.State.IsKeyDown(Keys.RightShift);
            ctrl = ks.State.IsKeyDown(Keys.LeftControl) || ks.State.IsKeyDown(Keys.LeftControl);
            alt = ks.State.IsKeyDown(Keys.LeftAlt) || ks.State.IsKeyDown(Keys.RightAlt);
            this.key = key;
        }

        public bool CanDecode()
        {
            if (DecodeKey() != (char)0)
                return true;

            return false;
        }

        public char DecodeKey()
        {
            if ((int)key >= (int)Keys.A && (int)key <= (int)Keys.Z)
            {
                return shift ? char.ToUpper((char)(int)key) : char.ToLower((char)(int)key);
            }

            if ((int)key >= (int)Keys.D0 && (int)key <= (int)Keys.D9)
            {
                int k = (int)key - (int)Keys.D0;

                if (!shift)
                    return k.ToString()[0];
                else
                {
                    switch (k)
                    {
                        case 1:
                            return '!';
                        case 2:
                            return '@';
                        case 3:
                            return '#';
                        case 4:
                            return '$';
                        case 5:
                            return '%';
                        case 6:
                            return '^';
                        case 7:
                            return '&';
                        case 8:
                            return '*';
                        case 9:
                            return '(';
                        case 0:
                            return ')';
                    }
                }
            }

            switch (key)
            {
                case Keys.OemComma:
                    return shift ? '<' : ',';
                case Keys.OemPeriod:
                    return shift ? '>' : '.';
                case Keys.OemQuestion:
                    return shift ? '?' : '/';
                case Keys.OemSemicolon:
                    return shift ? ':' : ';';
                case Keys.OemQuotes:
                    return shift ? '\"' : '\'';
                case Keys.OemOpenBrackets:
                    return shift ? '{' : '[';
                case Keys.OemCloseBrackets:
                    return shift ? '}' : ']';
                case Keys.OemMinus:
                    return shift ? '_' : '-';
                case Keys.OemPlus:
                    return shift ? '+' : '=';
                case Keys.OemPipe:
                    return shift ? '|' : '\\';
                case Keys.OemTilde:
                    return shift ? '~' : '`';
                case Keys.Space:
                    return ' ';
            }

            return (char)0;
        }

        public Keys Key { get { return key; } }
        public bool Shift { get { return shift; } }
        public bool Control { get { return ctrl; } }
        public bool Alt { get { return alt; } }
        public KeyboardManager KeyboardManager { get { return manager; } }
    }
}
