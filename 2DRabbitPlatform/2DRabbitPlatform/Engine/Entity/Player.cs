using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using _2DRabbitPlatform.GFX;

namespace _2DRabbitPlatform.Engine.Entity
{
    public class Player : HumanEntity
    {
        Animation walkW;
        bool canDamage;
        GameTimer damageTimer, moveTimer, blinkTimer;

        public Player(World world) 
            : base(world, 20)
        {
            init(new Rectangle(0, 0, 32, 32));
            walkW = new Animation(world.Game.TextureManager.rabbit_walkW);
            setAnimation(walkW);
            currentAnim.play();
            //base.setTexture(world.Game.TextureManager.rabbitMask);
            entityMask = EntityStaticMask.fromTexture(this, world.Game.TextureManager.rabbitMask);
            //entityMask = new EntityDynamicMask(this);
            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            Files.TileMaskFilePart.toStream(mem, EntityStaticMask.fromTexture(this, world.Game.TextureManager.rabbitMask));
            collisionArea = new Rectangle(0, 0, 32, 32);
            canDamage = true;
            damageTimer = createTimer();
            blinkTimer = createTimer();
            moveTimer = createTimer();
            mass = 100f;
        }

        public override void Draw(GFX.MapLayer layer)
        {
            base.Draw(layer);
            //sb.Draw(current, new Rectangle(Position.iX, position.iY, 32, 48), new Rectangle(0, 0, 64, 128), Color.White, 0f, Vector2.Zero, SpriteEffects.None, di.getDepth(Position + new Vector2(current.Width, current.Height)));
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);        
        }

        public override bool interact(Action.ActionArgs action)
        {
            if (action is Action.InteractEntityActionArgs)
            {
                IInteractable damager = ((Action.InteractEntityActionArgs)action).InteractedEntity;

                if (damager is Enemy)
                {
                    Enemy enemy = (Enemy)damager;
                    if (canDamage)
                    {
                        this.damageBy(enemy);
                        canDamage = false;
                        canMove = false;
                        damageTimer.setDelay(3500, delegate() { this.canDamage = true; this.blinkTimer.resetTimer(); this.color = Color.White; });
                        moveTimer.setDelay(1000, delegate() { this.canMove = true; });
                        blinkTimer.setRepeat(75, delegate() { if (this.color.A == 255) this.color = Color.Transparent; else this.color = Color.White; });
                    }
                    return true;
                }
            }

            return false;
        }

        public void shoot(StandardEntities entity)
        {
            Entity ent = world.EntityManager.createEntity(entity);
            ent.setLocation(this.Location);

            if (ent is MoveableEntity)
                ((MoveableEntity)ent).setVelocity(lastMoveDirection == MoveDirection.LEFT ? -15f + xvel : 15f + xvel, 0); //-1.5f + yvel);
        }
        
        public void damageBy(Enemy ent)
        {
            this.damage(ent.Damage);

            this.setVelocity((float)(Math.Cos(ent.Location.getDirectionTo(this.Location)) * 10), (float)(world.Random.NextDouble() * -3 - 3));
        }

        public void attack()
        {

        }

        public override Entity clone()
        {
            return new Player(world);
        }
    }
}
