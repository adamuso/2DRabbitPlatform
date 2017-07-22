using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Entity.Action
{
    public class ActionArgsCreator
    {
        World world;
        ActionType action;
        MoveableEntity sender;
        Location bufLocation;
        Entity bufEntity;

        public ActionArgsCreator(World world, MoveableEntity interactingEntity)
        {
            this.world = world;
            this.action = ActionType.NOTHING;
            this.sender = interactingEntity;
        }

        public void expandOther(ActionType action)
        {
            this.action |= action;
        }

        public void expandEntity(Entity entity)
        {
            if (entity == null)
                return;

            this.action |= ActionType.ENTITY;
            bufEntity = entity;
        }

        public void expandLocation(Location location)
        {
            this.bufLocation = location;
        }

        public void readuceOther(ActionType action)
        {
            this.action ^= action;
        }

        public void reduceEntity()
        {
            this.action ^= ActionType.ENTITY;
            bufEntity = null;
        }

        public bool isExpanded(ActionType action)
        {
            return (this.action & action) != 0;
        }

        public ActionArgs evaluate()
        {
            if (bufEntity is IInteractable)
            {
                IInteractable iable = (IInteractable)bufEntity;

                if (action == ActionType.INTERACT_ENTITY)
                    return new InteractEntityActionArgs(sender, iable);
            }

            return new ActionArgs(sender, ActionType.NOTHING);
        }

        public void reset()
        {
            action = ActionType.NOTHING;
            bufEntity = null;
        }

        public bool NothingToDo { get { return action == ActionType.NOTHING; } }
    }
}
