using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.AI
{
    public interface I_AI
    {
        I_AI addAIStep(I_AI step);
        I_AI addAISteps(params I_AI[] step);
        void setAIStep(int index, I_AI step);
        I_AI getAIStep(int index);
        void nextStep();
        void update();
        bool isDone { get; set; }
        void notify(AICommunicationSymbols symbols);
        bool notify(AICommunicationSymbols symbols, bool deepOnly);
        object communicate();
        object communicate(params object[] args);
    }
}
