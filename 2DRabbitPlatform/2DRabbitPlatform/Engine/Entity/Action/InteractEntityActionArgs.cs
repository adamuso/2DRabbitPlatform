using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Entity.Action
{
    public class InteractEntityActionArgs : ActionArgs
    {
        IInteractable interacted;

        public InteractEntityActionArgs(MoveableEntity interactingEntity, IInteractable interactedEntity)
            : this(interactingEntity, interactedEntity, ActionType.INTERACT_ENTITY) { }

        protected InteractEntityActionArgs(MoveableEntity interactingEntity, IInteractable interactedEntity, ActionType type)
            : base(interactingEntity, type)
        {
            this.interacted = interactedEntity;
        }

        public IInteractable InteractedEntity { get { return interacted; } }
    }
}
