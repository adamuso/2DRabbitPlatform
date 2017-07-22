using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine
{
    public class HandleTimerBase
    {
        private List<GameTimer> timers;

        public virtual void Update(GameTime gt)
        {
            if (timers != null)
                foreach (GameTimer t in timers)
                {
                    if (t.isDestroyed)
                    {
                        timers.Remove(t);

                        if (timers.Count == 0)
                            timers = null;

                        break;
                    }
                    else
                        t.Update(gt);
                }
        }

        protected void addTimer(GameTimer timer)
        {
            if (timers != null)
            {
                timers.Add(timer);
            }
            else
            {
                timers = new List<GameTimer>();
                timers.Add(timer);
            }
        }

        /// <summary>
        /// Tworzy wbudowany licznik, nie korzystający z osobnych wątków
        /// </summary>
        /// <returns>Licznik</returns>
        protected GameTimer createTimer()
        {
            GameTimer gt = new GameTimer();
            addTimer(gt);
            return gt;
        }

        protected GameTimer[] createTimer(int count)
        {
            List<GameTimer> ts = new List<GameTimer>();

            for (int i = 0; i < count; i++)
                ts.Add(createTimer());

            return ts.ToArray();
        }

        protected GameTimer getTimer(int id)
        {
            return timers[id];
        }
    }
}
