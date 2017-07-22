using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Entity
{
    public abstract class LivingEntity : MoveableEntity
    {
        int maxHealth;
        int health;

        public LivingEntity(World world, int maxHealth)
            : base(world)
        {
            this.maxHealth = maxHealth;
            this.health = maxHealth;
        }

        public virtual void damage(int damage)
        {
            health -= damage;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
            base.Update(gt);

            if (IsDead)
                destroy();
        }

        public int Health { get { return health; } }
        public int MaxHealth { get { return maxHealth; } }
        public bool IsDead { get { return health <= 0; } }
    }
}
