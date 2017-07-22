using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Event
{
    public enum EventType : int
    {
        PLAYER_SPAWN = 0,
        SPAWN = 1,
        STOP_AI,
        WARP,
        GENERATOR,
        ONE_WAY
    }
}
