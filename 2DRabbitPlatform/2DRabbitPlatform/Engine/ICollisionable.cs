using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using _2DRabbitPlatform.GFX;

namespace _2DRabbitPlatform.Engine
{
    public interface ICollisionable
    {
        Rectangle CollisionArea { get; }
        Mask Mask { get; }
        bool IsFlipped { get; }
    }
}
