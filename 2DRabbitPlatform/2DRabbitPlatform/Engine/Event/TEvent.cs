using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Event
{
    public interface TEvent : Files.IStoreable, Files.SerialBinary.IBinarySerializable
    {
        void Update(Microsoft.Xna.Framework.GameTime gt);
        void SetParameters(params object[] objects);
        bool IsDependent { get; }
        bool IsOneTimeEvent { get; }
        string Name { get; }
        string DisplayName { get; }
        bool Enabled { get; }
        int EventID { get; }
    }
}
