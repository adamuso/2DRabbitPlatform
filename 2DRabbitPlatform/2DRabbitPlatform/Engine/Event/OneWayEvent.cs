using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Event
{
    public class OneWayEvent : DependentEvent
    {
        public OneWayEvent() 
            : base()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt) { }
        public override void execute(World world, int x, int y) 
        {
            if (enabled)
            {
                ((EventTile)world.Level.getTile(x, y)).OneWayModifier = true;
                enabled = false;
            }
        }
        public override void execute(Entity.Entity executer, int x, int y) { }
        public override bool IsOneTimeEvent { get { return true; } }
        public override void toStream(System.IO.BinaryWriter writer) { }
        public override string Name { get { return "One way event"; } }
        public override string DisplayName { get { return "One\nWay"; } }
        public override int EventID { get { return (int)EventType.ONE_WAY; } }
    }
}
