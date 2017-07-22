using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.AI
{
    public class AIIdle : BaseAI
    {
        public AIIdle()
            : base()
        {

        }

        public override void notify(AICommunicationSymbols symbols)
        {
            if (symbols == AICommunicationSymbols.AI_STOP)
                isDone = true;
        }
    }
}
