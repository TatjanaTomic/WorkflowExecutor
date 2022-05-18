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
using System.Threading;

namespace CreatorMVVMProject.Model.Class.ExecutionService
{
    public class ExecutionService : IExecutionService
    {
        //ovdje ide logika za queue stepova koji idu na izvrsavanje
        //zamisljeno je kao lista lista
        //stepovi koji se izvrsavaju paralelno idu u jednu listu
        //za svaki step koji se izvrsava sekvencijalno pravi se lista koja ima jedan element
        //odavde se koristi step executor
        //queue prima step status

        //private static SemaphoreSlim semaphore;

        //public event EventHandler? StepsAdded;

        //private readonly ConcurrentQueue<List<StepStatus>> stepsQueue = new();

        private readonly BlockingCollection<StepStatus> stepsQueue = new();
        private readonly BlockingCollection<StepStatus> stepsQueueParallel = new();

        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly AutoResetEvent autoResetEvent = new(false);

        private readonly IStatusReportService statusReportService;

        public ExecutionService(IStatusReportService statusReportService)
        {
            this.statusReportService = statusReportService;
            //semaphore = new SemaphoreSlim(1);
            _ = StartExecution(() => ExecuteOneStepList());
        }

        private BlockingCollection<StepStatus> StepsQueue
        {
            get { return stepsQueue; }
        }

        private BlockingCollection<StepStatus> StepsQueueParallel
        {
            get { return stepsQueueParallel; }
        }
        
        public void EnqueueSteps(List<StepStatus> stepsToExecute)
        {

            Task addingSteps = Task.Run(() =>
            {

                foreach (StepStatus stepStatus in stepsToExecute)
                {
                    if (stepStatus.Step.CanBeExecutedInParallel)
                        StepsQueueParallel.Add(stepStatus);
                    else
                        StepsQueue.Add(stepStatus );
                }

            });

            addingSteps.Wait();

            autoResetEvent.Set();

            //StepsAdded?.Invoke(this, EventArgs.Empty);
        }

        public async Task StartExecution(Action action)
        {
            Task executingSteps = Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        if (cancellationTokenSource.IsCancellationRequested)
                            throw new OperationCanceledException();
                        action();
                        autoResetEvent.WaitOne();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    // An InvalidOperationException means that Take() was called on a completed collection
                }
            });

            await executingSteps;
        }
        

        public void ExecuteOneStepList()
        {
            while (StepsQueue.Any())
            {
                try
                {
                    StepStatus stepStatus = StepsQueue.Take();
                    AbstractExecutor? stepExecutor = ExecutorFabrique.Instance.CreateExecutor(stepStatus.Step);
                    if (stepExecutor != null)
                    {
                        stepExecutor.ExecutionStarted += StepExecutionStarted;
                        stepExecutor.ExecutionCompleted += StepExecutionCompleted;
                        stepExecutor.Start().Wait();
                    }
                }
                catch (Exception ex)
                {
                    //TODO : Sta se desava ako se ne izvrsi ?
                }
            }

            List<Task> steps = new();
            while (StepsQueueParallel.Any())
            {
                StepStatus stepStatus = StepsQueueParallel.Take();
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
