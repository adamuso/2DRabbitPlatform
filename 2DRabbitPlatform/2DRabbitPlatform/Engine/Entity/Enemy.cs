using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine.Entity
{
    public abstract class Enemy : LivingEntity, IEntityAI
    {
        AI.EntityAI ai;
        int damageval;
        GameTimer damageTimer, blinkTimer;
        bool canDamage;

        public Enemy(World world, int maxHealth) 
            : base(world, maxHealth)
        {
            ai = new AI.EntityAI(this);
            damageval = 1;
            damageTimer = createTimer();
            blinkTimer = createTimer();
            canDamage = true;
        }

        public override bool interact(Action.ActionArgs action)
        {
            if (action is Action.InteractEntityActionArgs)
            {
                IInteractable damager = ((Action.InteractEntityActionArgs)action).InteractedEntity;

                if (damager is Shuriken)
                {
                    Shuriken enemy = (Shuriken)damager;
                    if (canDamage)
                    {
                        //enemy.bounce();
                        enemy.attachToEntity(this);
                        canDamage = false;
                        damageTimer.setDelay(3500, delegate() { this.canDamage = true; this.blinkTimer.resetTimer(); this.color = Color.Pink; });
                        blinkTimer.setRepeat(75, delegate() { if (this.color.A == 255) this.color = Color.Transparent; else this.color = Color.Pink; });
                    }
                    return true;
                }
            }

            return false;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
            base.Update(gt);

            ai.nextStep();
        }

        public int Damage { get { return damageval; } }

        public AI.EntityAI AI { get { return ai; } }
    }
}
