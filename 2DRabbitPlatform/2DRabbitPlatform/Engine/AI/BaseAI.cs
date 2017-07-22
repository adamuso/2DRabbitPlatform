using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DRabbitPlatform.Engine.AI
{
    public class BaseAI : I_AI
    {
        List<I_AI> aiSteps;
        int current;
        bool done;

        public BaseAI() 
        {
            aiSteps = new List<I_AI>();
            current = 0;
            done = false;
        }

        public I_AI addAIStep(I_AI step)
        {
            aiSteps.Add(step);

            return this;
        }

        public I_AI addAISteps(params I_AI[] step)
        {
            aiSteps.AddRange(step);

            return this;
        }

        public void setAIStep(int index, I_AI step)
        {
            aiSteps[index] = step;
        }

        public I_AI getAIStep(int index)
        {
            return aiSteps[index];
        }

        public void nextStep()
        {
            if (aiSteps.Count > 0)
            {
                if (CurrentStep.isDone)
                {
                    CurrentStep.isDone = false;
                    current++;

                    if (current >= aiSteps.Count)
                        current = 0;
                }
                else
                {
                    CurrentStep.nextStep();
                    CurrentStep.update();
                }
            }
        }

        public virtual void update()
        {

        }

        public virtual object communicate()
        {
            if (aiSteps.Count > 0)
                return CurrentStep.communicate();
            else
                return null;
        }

        public virtual void notify(AICommunicationSymbols symbols)
        {

        }

        public bool notify(AICommunicationSymbols symbols, bool deepOnly)
        {
            if (aiSteps.Count > 0)
            {
                if (deepOnly)
                {
                    if (!CurrentStep.notify(symbols, deepOnly))
                    {
                        CurrentStep.notify(symbols);
                        return true;
                    }
                }
                else
                {
                    CurrentStep.notify(symbols);
                    CurrentStep.notify(symbols, deepOnly);
                    return true;
                }
            }

            return false;
        }

        public virtual object communicate(params object[] args)
        {
            if (aiSteps.Count > 0)
                return CurrentStep.communicate(args);
            else
                return null;
        }

        public bool isDone { get { return done; } set { done = value; } }
        public I_AI CurrentStep { get { return aiSteps[current]; } }
    }
}
