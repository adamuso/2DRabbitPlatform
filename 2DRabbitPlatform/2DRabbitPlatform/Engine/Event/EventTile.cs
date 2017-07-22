using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Event
{
    using Files.SerialBinary;

    public class EventTile : ExtendedTileData
    {
        TEvent[] events;
        int x, y;
        bool oneWayModifier;

        public EventTile(int id, bool flipped, int x, int y, params TEvent[] events)
            : base(id, flipped)
        {
            this.events = events;
            this.x = x;
            this.y = y;
            this.oneWayModifier = false;
        }

        public override void toStream(System.IO.BinaryWriter writer)
        {
            base.toStream(writer);

            if (HasEvents)
            {
                writer.Write(events.Length);

                for (int i = 0; i < events.Length; i++)
                {
                    writer.Write(events[i].EventID);
                    events[i].getSBCompound().writeCompound(writer);
                    events[i].toStream(writer);
                }
            }
        }

        protected override byte getExtendedFlags()
        {
            if (HasEvents)
            {
                byte ret = base.getExtendedFlags();
                ret = HasEvents ? (byte)(ret | (1 << 7)) : ret;

                return ret;
            }

            return base.getExtendedFlags();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
            base.Update(gt);

            if (HasEvents)
            {
                foreach (TEvent e in events)
                {
                    e.Update(gt);
                }
            }
        }


        public void execute(World world)
        {
            if (HasEvents)
            {
                foreach (TEvent e in events)
                {
                    if (e.IsDependent)
                        ((DependentEvent)e).execute(world, x, y);
                }
            }
        }

        public void execute(Entity.Entity entity)
        {
            if (HasEvents)
            {
                foreach (TEvent e in events)
                {
                    if (e.IsDependent)
                        ((DependentEvent)e).execute(entity, x, y);
                }
            }
        }

        public TEvent[] Events { get { return events; } }
        public override bool HasEvents { get { if (events != null) return events.Length > 0; else return false; } }
        public bool OneWayModifier { get { return oneWayModifier; } set { oneWayModifier = value; } }
    }
}
