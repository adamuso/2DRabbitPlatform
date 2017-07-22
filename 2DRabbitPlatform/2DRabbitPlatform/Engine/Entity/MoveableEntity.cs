using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine.Entity
{
    public abstract class MoveableEntity : AnimatedEntity, IInteractable
    {
        //private const float yacc = 0.001f;
        private const float yacc = 0.015f;
        protected float xvel, yvel, xFactor, maxXVel, maxYVel;
        protected Location bufferedPosition;
        private bool onGround;
        private bool gravityAffect;
        private bool jumpBoost;
        protected bool canMove;
        protected MoveDirection lastMoveDirection;
        protected float mass;

        protected MoveableEntity(World world)
            : base(world)
        {
            xvel = 0f;
            yvel = 0f;
            xFactor = 1f;
            bufferedPosition = position;
            gravityAffect = true;
            maxXVel = 4f;
            maxYVel = 10f;
            jumpBoost = false;
            canMove = true;
            lastMoveDirection = MoveDirection.LEFT;
            mass = 10f;
        }

        /// <summary>
        /// Spowalnia Entity o podane tarcie
        /// </summary>
        /// <param name="friction">Tarcie</param>
        public void slowdown(float friction)
        {
            xvel *= friction;
            //yvel *= friction;

            if (Math.Abs(xvel) < 0.001)
                xvel = 0;
            //if (Math.Abs(yvel) < 0.001)
            //    yvel = 0;
        }

        public void bufferPosition()
        {
            bufferedPosition = position;
        }

        public virtual void move(float x, float y)
        {
            bufferedPosition.move(x, y);

            bufferedPosition = new Location(world, Vector2.Clamp(bufferedPosition, Vector2.Zero, new Vector2(world.Level.Map.Width * GFX.Tile.STANDARD_GTILE_WIDTH, world.Level.Map.Height * GFX.Tile.STANDARD_GTILE_HEIGHT)));
        }

        public void goLeft(float powerFactor = 1f)
        {
            //if(Math.Abs(xvel) < SpeedMaxX)
            lastMoveDirection = MoveDirection.LEFT;
            addVelocity(-powerFactor * (2f/3f), 0);
            xvel = MathHelper.Clamp(xvel, -SpeedMaxX, SpeedMaxX);
        }

        public void goRight(float powerFactor = 1f)
        {
          //  if(Math.Abs(xvel) < SpeedMaxX)
            lastMoveDirection = MoveDirection.RIGHT;
            addVelocity(powerFactor * (2f/3f), 0);
            xvel = MathHelper.Clamp(xvel, -SpeedMaxX, SpeedMaxX);
        }

        public void jump()
        {
            float extra = Math.Abs(xvel / 15);

            if (onGround)
            {
                //setVelocity(xvel, -6.5f - Math.Abs(xvel / 15));
                setVelocity(xvel, -2.5f - extra);                
                jumpBoost = true;
            }

            if (jumpBoost && yvel > -6.5f - extra)
                addVelocity(0, -1.5f);

            if (jumpBoost && yvel <= -6.5f - extra)
            {
                jumpBoost = false;
                yvel = -6.5f - extra;
            }
        }

        public void jumpReset()
        {
            jumpBoost = false;
        }

        public void sprint(bool val)
        {
            xFactor = val ? 2f : 1f;
        }

        /// <summary>
        /// Ustawia prędkość Entity
        /// </summary>
        /// <param name="xv">Prędkość osi X</param>
        /// <param name="yv">Prędkość osi Y</param>
        public void setVelocity(float xv, float yv)
        {
            this.xvel = xv;
            this.yvel = yv;
        }

        public void addVelocity(float xv, float yv)
        {
            this.xvel += xv;
            this.yvel += yv;
        }

        /// <summary>
        /// Ustawia prędkość Entity
        /// </summary>
        /// <param name="degrees">Kierunek (w stopniach)</param>
        /// <param name="power">Siła</param>
        public void setDegVelocity(int degrees, float power)
        {
            this.xvel = (float)Math.Cos(MathHelper.ToRadians(degrees)) * power;
            this.yvel = (float)Math.Sin(MathHelper.ToRadians(degrees)) * -power;
        }

        /// <summary>
        /// Ustawia prędkość Entity
        /// </summary>
        /// <param name="radians">Kierunek (w radianach)</param>
        /// <param name="power">Siła</param>
        public void setRadVelocity(float radians, float power)
        {
            this.xvel = (float)Math.Cos(radians) * power;
            this.yvel = (float)Math.Sin(radians) * -power;
        }

        public void applyPosition()
        {
            base.position = bufferedPosition;
        }

        public void addGravityVelocity(GameTime gt)
        {
            yvel += yacc * gt.ElapsedGameTime.Milliseconds;
            yvel = MathHelper.Clamp(yvel, -maxYVel, maxYVel);
        }

        public void resetYVelocity()
        {
            this.yvel = 0;
        }

        public void resetXBuffer()
        {
            bufferedPosition.X = position.X;
        }

        public void resetYBuffer()
        {
            bufferedPosition.Y = position.Y;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            if (IsAttached)
            {
                return;
            }

            if(gravityAffect)
                addGravityVelocity(gt);
            bufferPosition();

            //slowdown(0.7f + (xFactor / 10f));

            moveHorizontal(gt);
            onGround = moveVertical(gt);

            if (!this.Location.offset(0, this.CollisionArea.Height + 5).getTile().isAir)
                slowdown(0.7f + (xFactor / 10f));
            else
                slowdown(0.85f + 1/mass + (xFactor / 20f));

            applyPosition();
        }

        /// <summary>
        /// Odziaływanie na inny obiekt
        /// </summary>
        /// <param name="action">Akcja</param>
        public virtual bool interact(Action.ActionArgs action) { return false; }
 
        public void moveHorizontal(GameTime gt)
        {
            int vel = (int)(Velocity.X * world.Game.getTimes(gt) * 10);     // uzyskaj ostateczną prędkość poprzez wyczulenie jej mnożąc przez 10 (doliczany czas tak naprawde nic nie zmienia)

            if (vel != 0)   // jeśli ma jakiś czas to wykonujemy ruch
            {
                int factor = vel / Math.Abs(vel);   // factor sluzy do oznaczenia ruchu w lewo i w prawo (lewo: -1f, prawo: 1f)
                vel = Math.Abs(vel);                // nastepnie usuwamy znak z prędkości aby mozna było jej użyć w normalnej pętli

                for (int i = 0; i < vel; i++)   // powtarzamy ilość ruchów jakie ma wykonać Entity
                {
                    move(0.1f * factor, 0);

                    if (world.CollisionManager.checkHorizontalMove(this, world.Level.Map) != 0)
                    {
                        for (int s = 1; s <= 2; s++)
                        {
                            move(0, (int)(-s * world.Game.getTimes(gt)));

                            if (world.CollisionManager.checkHorizontalMove(this, world.Level.Map) == 0)
                            {
                                break;
                            }

                            if (s < 2)
                                continue;

                            resetYBuffer();

                            move(-1 * factor, 0);
                            xvel = 0;
                            if (this is IEntityAI)
                                ((IEntityAI)this).AI.notify(AI.AICommunicationSymbols.AI_STOP, false);
                            goto escape;
                        }
                    }
                    else
                    {
                        for (int s = 1; s <= 3; s++)
                        {
                            move(0, 1);

                            if (world.CollisionManager.checkHorizontalMove(this, world.Level.Map) != 0)
                            {
                                move(0, -1);
                                break;
                            }

                            if (s == 3)
                                move(0, -s);
                        }
                    }

                }

            escape:
                position.Y = bufferedPosition.Y;
            }
        }

        public bool moveVertical(GameTime gt)
        {
            int vel = (int)(Velocity.Y * world.Game.getTimes(gt) * 10);

            if (vel != 0)
            {
                int factor = vel / Math.Abs(vel);
                vel = Math.Abs(vel);

                for (int i = 0; i < vel; i++)
                {
                    move(0, 0.1f * factor);
                    int height = world.CollisionManager.checkVerticalMove(this, world.Level.Map);

                    if (height != 0)
                    {
                        resetYBuffer();
                        resetYVelocity();

                        if (height > 0)
                        {
                            yvel += 1f;
                            move(0, 1);
                            jumpBoost = false;
                        }

                        if (height < 0)
                            return true;

                        break;
                    }
                }
            }

            return false;
        }
#if OLD
        public void moveHorizontal(GameTime gt)
        {
            int vel = (int)(Velocity.X * world.Game.getTimes(gt));

            if (vel != 0)
            {
                int factor = vel / Math.Abs(vel);
                vel = Math.Abs(vel);

                int height = 0;

                for (int i = 0; i < vel; i++)
                {
                    move(1 * factor, 0);

                    height = world.CollisionManager.checkHorizontalMove(this, world.Level);

                    Console.WriteLine("X: " + bufferedPosition.iX + ", Y: " + bufferedPosition.iY + ", H: " + height);

                    if (height >= -3 && height < 0)
                        move(0, height);

                    if (height < -3 || height > 6)
                    {
                        resetXBuffer();
                        xvel = factor / 2;
                        if (this is IEntityAI)
                            ((IEntityAI)this).AI.communicate();
                        break;
                    }
                }

                if (world.CollisionManager.checkHorizontalMove(this, world.Level) < -3)
                {
                    resetXBuffer();
                    xvel = 0;
                }

                position.Y = bufferedPosition.Y;
            }
        }

        public bool moveVertical(GameTime gt)
        {
            int vel = (int)(Velocity.Y * world.Game.getTimes(gt));

            if (vel != 0)
            {
                int factor = vel / Math.Abs(vel);
                vel = Math.Abs(vel);

                for (int i = 0; i < vel; i++)
                {
                    move(0, 1 * factor);
                    int height = world.CollisionManager.checkVerticalMove(this, world.Level);

                    Console.WriteLine("VERT: X: " + bufferedPosition.iX + ", Y: " + bufferedPosition.iY + ", H: " + height);

                    if (height != 0)
                        break;

                }

                int height2 = world.CollisionManager.checkVerticalMove(this, world.Level);

                if (height2 != 0)
                {
                    resetYVelocity();
                    resetYBuffer();

                    if (height2 > 0)
                    {
                        yvel += 1f;
                        move(0, height2);
                        jumpBoost = false;
                    }

                    if (height2 < 0)
                        return true;
                }
            }

            return false;
        }
#endif
        public override Location Location
        {
            get
            {
                return bufferedPosition;
            }
            set
            {
                base.Location = value;
            }
        }
        /// <summary>
        /// Obszar kolizji obiektu
        /// </summary>
        public override Rectangle CollisionArea
        {
            get
            {
                return new Rectangle(bufferedPosition.iX + collisionArea.X, bufferedPosition.iY + collisionArea.Y, collisionArea.Width, collisionArea.Height);
            }
        }
        private float SpeedMaxX { get { return maxXVel * xFactor; } }
        public Vector2 Velocity { get { return new Vector2(xvel, yvel); }}
        public bool AffectedByGravity { get { return gravityAffect; } set { gravityAffect = value; } }
        public bool CanMove { get { return canMove; } }

        protected enum MoveDirection
        {
            LEFT = 0,
            RIGHT = 1
        }
    }
}
