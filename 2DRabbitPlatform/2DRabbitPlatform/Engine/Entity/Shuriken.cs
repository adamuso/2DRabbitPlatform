using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine.Entity
{
    public class Shuriken : MoveableEntity
    {
        public Shuriken(World world)
            : base(world)
        {
            init(new Rectangle(0, 0, 15, 14));
            setAnimation(new GFX.Animation(world.Game.TextureManager.shuriken_anim));
            currentAnim.play();
            AffectedByGravity = false;
            collisionArea = new Rectangle(0, 0, 15, 14);
            entityMask = EntityStaticMask.fromTexture(this, world.Game.TextureManager.shurikenMask);
            createTimer().setDelay(2000, delegate() { this.destroy(); });
            createTimer().setDelay(300, delegate() { AffectedByGravity = true; });
        }

        public override Entity clone()
        {
            return new Shuriken(this.world);
        }

        public void bounce()
        {
            if (xvel != 0)
            {
                float factor = xvel / Math.Abs(xvel);
                xvel = -factor / 2;
                yvel = -3;
            }
        }
    }
}
