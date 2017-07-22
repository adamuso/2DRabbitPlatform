using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Files
{
    public interface IStoreable
    {
        void toStream(System.IO.BinaryWriter writer);
    }
}
