using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Event
{
    public abstract class DependentEvent : TEvent
    {
        protected bool enabled;

        public DependentEvent()
        {
            enabled = true;
        }

        public abstract void Update(Microsoft.Xna.Framework.GameTime gt);
        public bool IsDependent { get { return true; } }
        public abstract void execute(World world, int x, int y);
        public abstract void execute(Entity.Entity executer, int x, int y);
        public abstract bool IsOneTimeEvent { get; }
        public bool Enabled { get { return enabled; } }
        public abstract string Name { get; }
        public abstract string DisplayName { get; }
        public abstract int EventID { get; }
        public virtual void toStream(System.IO.BinaryWriter writer) { }
        public virtual void SetParameters(params object[] objects) { }
    }
}
