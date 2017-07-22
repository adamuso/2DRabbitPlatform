using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2DRabbitPlatform.GFX;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine
{
    public class EntityMask : Mask
    {
        protected Entity.Entity entity;

        protected EntityMask(Entity.Entity entity)
            : base(entity.DrawingArea.Width, entity.DrawingArea.Height)
        {
            this.entity = entity;
        }
    }
}
