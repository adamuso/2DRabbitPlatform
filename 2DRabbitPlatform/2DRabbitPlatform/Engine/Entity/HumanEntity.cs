using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Entity
{
    public abstract class HumanEntity : LivingEntity
    {
        public HumanEntity(World world, int maxHealth)
            : base(world, maxHealth)
        {

        }
    }
}
