using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2DRabbitPlatform.GFX;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine.Entity
{
    public class EnemyWolf : Enemy
    {
        public EnemyWolf(World world)
            : base(world, 10)
        {
            init(new Microsoft.Xna.Framework.Rectangle(0, 0, 32, 32));
            setAnimation(new Animation(world.Game.TextureManager.rabbit_walkW));
            currentAnim.play();
            entityMask = EntityStaticMask.fromTexture(this, world.Game.TextureManager.rabbitMask);
            collisionArea = new Microsoft.Xna.Framework.Rectangle(0, 0, 32, 32);
            color = Color.Pink;
            AI.addAIStep(new AI.EntityAIFlipAnimation(this, true));
            AI.addAIStep(new AI.EntityAIMoveUntilStopped(this, 0.35f, false));
            AI.addAIStep(new AI.EntityAIFlipAnimation(this, false));
            AI.addAIStep(new AI.EntityAIMoveUntilStopped(this, 0.35f, true));
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
            base.Update(gt);
        }

        public override Entity clone()
        {
            return new EnemyWolf(world);
        }
    }
}
