using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.AI
{
    public class EntityAIFlipAnimation : EntityAI
    {
        bool flipped;

        public EntityAIFlipAnimation(Entity.Entity entity, bool isFlipped)
            : base(entity)
        {
            this.flipped = isFlipped;
        }

        public override void update()
        {
            entity.IsFlipped = flipped;
            isDone = true;
        }
    }
}
