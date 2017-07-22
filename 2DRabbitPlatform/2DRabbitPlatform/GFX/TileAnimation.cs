using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.GFX
{
    public class TileAnimation : Engine.Event.EventTile
    {
        int[] ids;
        int interval;
        bool pingpong, pong;
        int current;

        public TileAnimation(int[] ids, int interval, bool pingpong, bool flipped, int x, int y, Engine.Event.TEvent[] events)
            : base(ids[0], flipped, x, y, events)
        {
            this.ids = ids;
            this.interval = interval;
            this.pingpong = pingpong;
            this.pong = false;
            this.current = 0;
            createTimer().setRepeat(interval, step);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
            base.Update(gt);

            base.ID = ids[current];
        }

        public void step()
        {
            if (!pingpong)
            {
                if (current < ids.Length - 1)
                    current++;
                else
                    current = 0;
            }
            else
            {
                if (current < ids.Length - 1 && !pong)
                    current++;
                else
                    pong = true;

                if (pong && current > 0)
                    current--;
                else
                    pong = false;
            }
        }

        public override void toStream(System.IO.BinaryWriter writer)
        {
            base.toStream(writer);

            writer.Write(ids.Length);
            for (int i = 0; i < ids.Length; i++)
                writer.Write(ids[i]);
            writer.Write(interval);
            writer.Write(pingpong);
        }

        protected override byte getExtendedFlags()
        {
            byte ret = base.getExtendedFlags();
            ret = (byte)(ret | (1 << 6));

            return ret;
        }
    }
}
