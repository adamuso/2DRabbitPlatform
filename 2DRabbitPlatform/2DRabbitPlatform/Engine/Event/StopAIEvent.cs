using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Event
{
    public class StopAIEvent : DependentEvent
    {
        public StopAIEvent()
            : base()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt) { }

        public override void execute(World world, int x, int y) { }

        public override void execute(Entity.Entity executer, int x, int y)
        {
            if (executer is Entity.IEntityAI)
            {
                Entity.IEntityAI iai = (Entity.IEntityAI)executer;
                iai.AI.notify(AI.AICommunicationSymbols.AI_STOP, false);
            }
        }

        public override void toStream(System.IO.BinaryWriter writer) { }

        public override bool IsOneTimeEvent { get { return false; } }

        public override string Name { get { return "Stop AI event"; } }

        public override string DisplayName { get { return "Stop\nAI"; } }

        public override int EventID { get { return (int)EventType.STOP_AI; } }
    }
}
