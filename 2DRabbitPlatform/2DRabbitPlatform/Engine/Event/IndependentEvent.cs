using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Event
{
    public abstract class IndependentEvent : HandleTimerBase, TEvent
    {
        protected bool enabled;
        protected int x, y;
        protected World world;

        public IndependentEvent(World world, int x, int y)
        {
            enabled = true;
            this.world = world;
            this.x = x;
            this.y = y;
        }

        public abstract void execute();
        //public abstract void toStream(System.IO.BinaryWriter writer);
        public bool IsDependent { get { return false; } }
        public abstract bool IsOneTimeEvent { get; }
        public bool Enabled { get { return enabled; } }
        public abstract string Name { get; }
        public abstract string DisplayName { get; }
        public abstract int EventID { get; }
        public virtual void toStream(System.IO.BinaryWriter writer) { }
        public virtual void SetParameters(params object[] objects) { }
    }
}
