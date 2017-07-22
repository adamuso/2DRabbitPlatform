using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.AI
{
    public class EntityAI : BaseAI
    {
        protected Entity.Entity entity;

        public EntityAI(Entity.Entity entity)
        {
            this.entity = entity;
        }

        public Entity.Entity Entity { get { return entity; } }
    }
}
