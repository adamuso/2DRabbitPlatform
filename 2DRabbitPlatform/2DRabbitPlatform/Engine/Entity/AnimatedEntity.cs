using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2DRabbitPlatform.GFX;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DRabbitPlatform.Engine.Entity
{
    public abstract class AnimatedEntity : Entity
    {
        protected Animation currentAnim;
        protected GameTimer animationTimer;
        protected SpriteEffects spriteEffect;

        protected AnimatedEntity(World world)
            : base(world)
        {
            current = null;
            spriteEffect = SpriteEffects.None;
            animationTimer = createTimer();
            animationTimer.setRepeat(50, updateAnim);
        }

        /// <summary>
        /// Ustawia aktualnie odtwarzaną animacje
        /// </summary>
        /// <param name="val">Animacja</param>
        public void setAnimation(Animation val)
        {
            setTexture(val.Texture.Source);
            currentAnim = val;
        }

        /// <summary>
        /// Aktualizuje animacje
        /// </summary>
        protected virtual void updateAnim()
        {
            currentAnim.Update();
        }

        public override void Draw(GFX.MapLayer layer)
        {
            rotationCenter = new Vector2(currentAnim.Texture[currentAnim.Current].Width / 2, currentAnim.Texture[currentAnim.Current].Height / 2);

            layer.Draw(current, position + rotationCenter, currentAnim.Texture[currentAnim.Current], color, rotation, rotationCenter, 1f, spriteEffect | (IsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None), layer.Depth);
        }

        /// <summary>
        /// Prędkość odtwarzania animacji
        /// </summary>
        protected int Interval { get { return animationTimer.Delay; } set { animationTimer.Delay = value; } }
        ///// <summary>
        ///// Pozycja obiektu wzynaczana z prawego dolnego rogu
        ///// </summary>
        //public override Location Location { get { return new Location(world, Location + new Vector2(currentAnim.Width, currentAnim.Height)); } }
        ///// <summary>
        ///// Środek obiektu
        ///// </summary>
        //public override Location Center { get { return new Location(world, Location + new Vector2(currentAnim.Width / 2, currentAnim.Height / 2)); } }
        /// <summary>
        /// Obszar kolizji obiektu
        /// </summary>
        public override Rectangle DrawingArea { get { return new Rectangle(Location.iX, Location.iY, currentAnim.Width, currentAnim.Height); } }
        public Animation Animation { get { return currentAnim; } }
        public SpriteEffects SpriteEffect { set { spriteEffect = value; } }
    }
}
