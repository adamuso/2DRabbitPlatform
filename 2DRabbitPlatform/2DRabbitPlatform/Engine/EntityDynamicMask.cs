using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DRabbitPlatform.Engine
{
    public class EntityDynamicMask : EntityMask
    {
        Texture2D buf;
        Color[] databuf;

        public EntityDynamicMask(Entity.Entity entity)
            : base(entity)
        {
            buf = null;
        }

        public override byte getByte(int index, bool flipped = false)
        {
            if (entity.RawTexture != buf)
            {
                buf = entity.RawTexture;
                Rectangle rect = entity.RawTexture.Bounds;

                if (entity is Entity.AnimatedEntity)
                {
                    Entity.AnimatedEntity anim = (Entity.AnimatedEntity)entity;
                    rect = anim.Animation.Texture[anim.Animation.Current];
                }

                databuf = new Color[rect.Width * rect.Height];
                entity.RawTexture.GetData<Color>(0, rect, databuf, 0, rect.Width * rect.Height);
            }

            if (!flipped)
                return databuf[index].A == 255 ? (byte)1 : (byte)0;

            int y = index / width;
            int x = width - (index - y * width) - 1;

            return databuf[y * width + x].A == 255 ? (byte)1 : (byte)0;
        }
    }
}
