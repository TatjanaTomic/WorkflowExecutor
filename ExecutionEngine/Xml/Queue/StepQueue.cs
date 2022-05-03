using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Xml.Queue
{
    public sealed class StepQueue
    {
        private readonly Queue<Step> parallelSteps = new();
        private readonly Queue<Step> sequentSteps = new();

        private static readonly StepQueue instance = new();


        static StepQueue()
        {

        }
        private StepQueue()
        {

        }

        public static StepQueue Instance { get { return instance; } }

        public void BuildQueue(StageList stageList)
        {
            if(stageList != null && stageList.Stages != null)
            {
                foreach(var stage in stageList.Stages)
                {
                    if(stage.Steps != null)
                    {
                        foreach(var step in stage.Steps)
                        {
                            if(step.CanBeExecutedInParallel == true)
                                parallelSteps.Enqueue(step);
                            else
                                sequentSteps.Enqueue(step);
                        }
                    }
                }
            }
        }

        public void ExecuteParallelSteps()
        {
            Console.WriteLine("Executing...");

            foreach(var step in parallelSteps)
            {
                new Thread(() =>
                {
                    //Thread.CurrentThread.IsBackground = true;
                    Console.WriteLine(step.Id + " started.");
                    step.Execute();
                    Console.WriteLine(step.Id + " finished.");
                }).Start();
                
            }
        }
    }
}
