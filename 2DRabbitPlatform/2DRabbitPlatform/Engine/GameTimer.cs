using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine
{
    public class GameTimer
    {
        Delegate action;
        object parameter, returnvalue;
        int delay, timesOfRepeat;
        bool repeat, enabled, retValue, paused, destroyed;

        int miliseconds;

        public GameTimer()
        {
            destroyed = false;
            action = null;
            parameter = null;
            returnvalue = null;
            delay = 0;
            timesOfRepeat = -1;
            repeat = false;
            enabled = false;
            miliseconds = 0;
            retValue = false;
            paused = false;
        }

        public void resetTimer()
        {
            action = null;
            parameter = null;
            delay = 0;
            timesOfRepeat = -1;
            repeat = false;
            enabled = false;
            miliseconds = 0;
            retValue = false;
            paused = false;
        }

        public void Update(GameTime gt)
        {
            if (enabled && !paused && delay > 0)
            {
                miliseconds += gt.ElapsedGameTime.Milliseconds;

                if (miliseconds > delay)
                {
                    if (repeat)
                    {
                        if (timesOfRepeat > 0)
                        {
                            action.DynamicInvoke();
                            miliseconds = 0;
                            timesOfRepeat--;
                        }
                        else if (timesOfRepeat == -1)
                        {
                            action.DynamicInvoke();
                            miliseconds = 0;
                        }
                        else
                            resetTimer();
                    }
                    else
                    {
                        if (retValue)
                            if(parameter != null)
                                returnvalue = action.DynamicInvoke(parameter);
                            else
                                returnvalue = action.DynamicInvoke();
                        else
                            if (parameter != null)
                                action.DynamicInvoke(parameter);
                            else
                                action.DynamicInvoke();
                        resetTimer();
                    }
                }
            }
        }

        public void setRepeat(int interval, Action callMethod)
        {
            action = callMethod;
            delay = interval;
            repeat = true;
            enabled = true;
        }

        public void setRepeat(int interval, int repeats, Action callMethod)
        {
            setRepeat(interval, callMethod);
            timesOfRepeat = repeats;
        }

        public void setDelay(int delay, Action callMethod)
        {
            action = callMethod;
            parameter = null;
            this.delay = delay;
            repeat = false;
            enabled = true;
        }

        public void setDelay<T>(int delay, Action<T> callMethod, T parameter)
        {
            action = callMethod;
            this.parameter = parameter;
            this.delay = delay;
            repeat = false;
            enabled = true;
        }

        public void setDelay<T>(int delay, Func<T> callFunction)
        {
            action = callFunction;
            this.parameter = null;
            this.delay = delay;
            this.returnvalue = null;
            repeat = false;
            enabled = true;
            retValue = true;
        }

        public void setDelay<T>(int delay, Func<T> callFunction, T parameter)
        {
            action = callFunction;
            this.parameter = parameter;
            this.delay = delay;
            this.returnvalue = null;
            repeat = false;
            enabled = true;
            retValue = true;
        }

        public T getReturnValue<T>()
        {
            return (T)returnvalue;
        }

        public void pause()
        {
            paused = false;
        }

        public void resume()
        {
            paused = true;
        }

        public void destroy()
        {
            destroyed = true;
        }

        public void stop()
        {
            enabled = false;
            miliseconds = 0;
        }

        public void start()
        {
            enabled = true;
            miliseconds = 0;
        }

        public bool isEnabled { get { return enabled; } }
        public bool isDestroyed { get { return destroyed; } }
        public int Delay { get { return delay; } set { delay = value; } }
    }
}
