using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.AI
{
    public class EntityAIMoveUntilStopped : EntityAI
    {
        float power;
        bool direction;

        public EntityAIMoveUntilStopped(Entity.MoveableEntity entity, float power = 1f, bool left = false)
            : base(entity)
        {
            this.power = power;
            this.direction = left;
        }

        public override void update()
        {
            if (!direction)
                ((Entity.MoveableEntity)entity).goRight(power);
            else
                ((Entity.MoveableEntity)entity).goLeft(power);
        }

        public override void notify(AICommunicationSymbols symbols)
        {
            if (symbols == AICommunicationSymbols.AI_STOP)
                isDone = true;
        }
    }
}
