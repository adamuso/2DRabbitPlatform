using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2DRabbitPlatform.GFX;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine.Entity
{
    public class ItemCarrot : Item
    {
        public ItemCarrot(World world)
            : base(world, "Carrot")
        {
            init(new Rectangle(0, 0, 20, 20));
            setTexture(TextureManager.Instance.i_carrot);
        }

        public override Entity clone()
        {
            return new ItemCarrot(world);
        }

        public override bool interact(Action.ActionArgs action)
        {
            throw new NotImplementedException();
        }
    }
}
