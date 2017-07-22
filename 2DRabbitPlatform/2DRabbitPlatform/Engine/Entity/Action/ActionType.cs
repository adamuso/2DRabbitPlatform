using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Entity.Action
{
    public enum ActionType 
    {
        NOTHING = 0,
        ENTITY = 1 << 0,
        GROUND = 1 << 1,
        INTERACT = 1 << 2,
        ITEM = 1 << 3,
        TOOL = 1 << 4,
        INTERACT_ENTITY = ENTITY | INTERACT,
        INTERACT_ENTITY_WITH_ITEM = ENTITY | INTERACT | ITEM,
        INTERACT_ENTITY_WITH_TOOL = ENTITY | INTERACT | TOOL,
        DROP_ITEM = GROUND | ITEM
    }
}
