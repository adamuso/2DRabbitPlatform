using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _2DRabbitPlatform.Engine
{
    public class EntityStaticMask : EntityMask
    {
        public EntityStaticMask(Entity.Entity entity) 
            : base(entity)
        {

        }

        public static EntityStaticMask fromTexture(Entity.Entity entity, Texture2D texture)
        {
            EntityStaticMask mask = new EntityStaticMask(entity);
            Color[] colors = new Color[mask.width * mask.height];
            texture.GetData<Color>(colors);

            for (int y = 0; y < mask.height; y++)
                for (int x = 0; x < mask.width; x++)
                {
                    if (colors[x + y * mask.width] == Color.Black)
                    {
                        mask.mask[x + y * mask.width] = 1;
                    }
                }

            return mask;
        }
    }
}
