using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.Entity
{
    public interface IInteractable
    {
        bool interact(Action.ActionArgs action);
    }
}
