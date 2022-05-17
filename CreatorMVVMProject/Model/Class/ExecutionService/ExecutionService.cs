using CreatorMVVMProject.Model.Interface.ExecutionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using ExecutionEngine.Xml;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using CreatorMVVMProject.Model.Class.StatusReportService;
using ExecutionEngine.Executor;
using ExecutionEngine.Step;

namespace CreatorMVVMProject.Model.Class.ExecutionService
{
    public class ExecutionService : IExecutionService
    {
        //ovdje ide logika za queue stepova koji idu na izvrsavanje
        //zamisljeno je kao lista lista
        //stepovi koji se izvrsavaju paralelno idu u jednu listu
        //za svaki step koji se izvrsava sekvencijalno pravi se lista koja ima jedan element
        //odavde se koristi step executor
        //queue prima step statuse

        private readonly IStatusReportService statusReportService;

        //private readonly ConcurrentQueue<List<StepStatus>> stepsQueue = new();
        private readonly BlockingCollection<List<StepStatus>> stepsQueue = new();

        public ExecutionService(IStatusReportService statusReportService)
        {
            this.statusReportService = statusReportService; StartExecution();
        }

        private BlockingCollection<List<StepStatus>> StepsQueue
        {
            get { return stepsQueue; }
        }
        
        public void EnqueueSteps(List<StepStatus> stepsToExecute)
        {
            Task addingSteps = Task.Run(() =>
            {
                List<StepStatus> parallelSteps = new();

                foreach (StepStatus stepStatus in stepsToExecute)
                {
                    if (stepStatus.Step.CanBeExecutedInParallel)
                        parallelSteps.Add(stepStatus);
                    else
                        StepsQueue.Add(new List<StepStatus> { stepStatus });
                }
                StepsQueue.Add(parallelSteps);

            });

            addingSteps.Wait();

            
        }

        public async Task StartExecution()
        {
            Task executingSteps = Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        ExecuteOneStepList(stepsQueue.Take());
                    }
                }
                catch (InvalidOperationException ex)
                {
                    // An InvalidOperationException means that Take() was called on a completed collection
                }
            });

            await executingSteps;
        }
        

        public void ExecuteOneStepList(List<StepStatus> stepsToExecute)
        {
            List<Task> steps = new();

            foreach (StepStatus stepStatus in stepsToExecute)
            {
                try
                {
                    AbstractExecutor? stepExecutor = ExecutorFabrique.Instance.CreateExecutor(stepStatus.Step);
                    if (stepExecutor != null)
                    {
                        stepExecutor.ExecutionStarted += StepExecutionStarted;
                        stepExecutor.ExecutionCompleted += StepExecutionCompleted;
                        steps.Add(stepExecutor.Start());
                     }
                }
                catch (Exception ex)
                {
                    //TODO : Sta se desava ako se ne izvrsi ?
                }
            }

            Task.WaitAll(steps.ToArray());
        }

        private void StepExecutionStarted(object? _, Step e)
        {
            statusReportService.SetStatusToStep(e, Status.InProgress);
        }

        private void StepExecutionCompleted(object? _, ExecutionCompletedEventArgs args)
        {
            if(args.IsSuccessful)
                statusReportService.SetStatusToStep(args.Step, Status.Success);
            else
                statusReportService.SetStatusToStep(args.Step, Status.Failed);
        }

    }
}
