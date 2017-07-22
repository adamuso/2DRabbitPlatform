using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Entity
{
    public abstract class Item : Entity, IInteractable
    {
        string name;
        float sin, angle;

        protected Item(World world, string name) : base(world) { this.name = name; this.angle = -Microsoft.Xna.Framework.MathHelper.Pi; }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
            base.Update(gt);

            angle += Microsoft.Xna.Framework.MathHelper.Pi / 35;
            sin = (float)Math.Sin(angle);

            if (angle > Microsoft.Xna.Framework.MathHelper.Pi)
                angle = -Microsoft.Xna.Framework.MathHelper.Pi;

            offset.Y = sin * 4f;
        }

        public abstract bool interact(Action.ActionArgs action);

        public  string Name { get { return name; } }
    }
}
