using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Entity.Action
{
    public class ActionArgs
    {
        MoveableEntity sender;
        ActionType type;

        public ActionArgs(MoveableEntity interactingEntity, ActionType type)
        {
            this.sender = interactingEntity;
            this.type = type;
        }

        public static bool hasInteractingEntity(ActionArgs action) { return action is InteractEntityActionArgs; }

        public MoveableEntity InteractingEntity { get { return sender; } }
        public ActionType Type { get { return type; } }
    }
}
