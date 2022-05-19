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
using CreatorMVVMProject.Model.Interface.WorkflowService;

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

        private readonly BlockingCollection<StepStatus> stepsQueue = new();
        private readonly BlockingCollection<StepStatus> stepsQueueParallel = new();

        //CancellationTokenSource moze se iskoristiti za zaustavljanje
        private readonly CancellationTokenSource cancellationTokenSource = new();
        //Represents a thread synchronization event that, when signaled, resets automatically after releasing a single waiting thread. 
        private readonly AutoResetEvent autoResetEvent = new(false);

        private readonly IStatusReportService statusReportService;
        private readonly IWorkflowService workflowService;

        public ExecutionService(IStatusReportService statusReportService, IWorkflowService workflowService)
        {
            this.statusReportService = statusReportService;
            this.workflowService = workflowService;

            _ = StartExecution();
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
            Task.Run(() =>
            {
                foreach (StepStatus stepStatus in stepsToExecute)
                {
                    if (stepStatus.Step.CanBeExecutedInParallel)
                        StepsQueueParallel.Add(stepStatus);
                    else
                        StepsQueue.Add(stepStatus );

                    statusReportService.SetStatusToStep(stepStatus, Status.Waiting);
                }

            }).Wait();

            autoResetEvent.Set();
        }

        public async Task StartExecution()
        {
            Task executingSteps = Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        //if (cancellationTokenSource.IsCancellationRequested)
                        //    throw new OperationCanceledException();
                        //TODO : Ovo se moze iskoristiti za neki Cancel
                        ExecuteAll();
                        autoResetEvent.WaitOne();
                    }
                }
                catch (InvalidOperationException)
                {
                    // An InvalidOperationException means that Take() was called on a completed collection
                }
            });

            await executingSteps;
        }

        //TODO : Preimenuj metodu
        //TODO : Valjalo bi refaktorisati kod
        public void ExecuteAll()
        {
            ExecuteParallelSteps(StepsQueueParallel);
            ExecuteSerialSteps(StepsQueue);
            
            /*
            while (StepsQueue.Any())
            {
                try
                {
                    StepStatus stepStatus = StepsQueue.Take();
                    AbstractExecutor stepExecutor = CreateStepExecutor(stepStatus.Step);
                    stepExecutor.Start().Wait();
                }
                catch (Exception)
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
                    AbstractExecutor stepExecutor = CreateStepExecutor(stepStatus.Step);
                    steps.Add(stepExecutor.Start());
                }
                catch (Exception)
                {
                    //TODO : Sta se desava ako se ne izvrsi ?
                }
            }
            Task.WaitAll(steps.ToArray());
            */
        }

        private void ExecuteSerialSteps(BlockingCollection<StepStatus> serialSteps)
        {
            while (serialSteps.Any())
            {
                try
                {
                    StepStatus stepStatus = serialSteps.Take();
                    AbstractExecutor stepExecutor = CreateStepExecutor(stepStatus.Step);
                    stepExecutor.Start().Wait();
                }
                catch (Exception)
                {
                    //TODO : Sta se desava ako se ne izvrsi ?
                }
            }
        }

        private void ExecuteParallelSteps(BlockingCollection<StepStatus> parallelSteps)
        {
            List<Task> tasks = new();
            while (parallelSteps.Any())
            {
                StepStatus stepStatus = parallelSteps.Take();
                try
                {
                    AbstractExecutor stepExecutor = CreateStepExecutor(stepStatus.Step);
                    tasks.Add(stepExecutor.Start());
                }
                catch (Exception)
                {
                    //TODO : Sta se desava ako se ne izvrsi ?
                }
            }
            Task.WaitAll(tasks.ToArray());
        }
        /*
        private void Test(StepStatus stepStatus)
        {
            BlockingCollection<StepStatus> serialSteps = new();
            BlockingCollection<StepStatus> parallelSteps = new();

            List<Step> allDependencySteps = workflowService.GetAllDependencySteps(stepStatus.Step);

            foreach (Step step in allDependencySteps)
            {
                StepStatus stepStatus2 = statusReportService.GetStepStatus(step);
                if (stepStatus2.Status == Status.NotStarted)
                {
                    if (step.CanBeExecutedInParallel == true)
                        parallelSteps.Add(stepStatus2);
                    else
                        serialSteps.Add(stepStatus2);
                }
                else
                {
                    Test(stepStatus2);
                }
            }

            ExecuteParallelSteps(parallelSteps);
            ExecuteSerialSteps(serialSteps);
        }
        */
        public async void StartExecuteTillThisStep(StepStatus stepStatus)
        {
            Task executingTask = Task.Run(() =>
            {
                ExecuteTillThisStep(stepStatus);
            });

            await executingTask;
        }

        private void ExecuteTillThisStep(StepStatus stepStatus)
        {
            /*
            if(stepStatus.Status == Status.NotStarted)
            {
                ExecuteOneStep(stepStatus);
            }
            else
            {
                List<Step> allDependencySteps = workflowService.GetAllDependencySteps(stepStatus.Step);

                foreach (Step step in allDependencySteps)
                {
                    StepStatus stepStatus2 = statusReportService.GetStepStatus(step);
                    ExecuteTillThisStep(stepStatus2);
                }
            }
            */

            List<Step> allSteps = workflowService.GetAllDependencySteps(stepStatus.Step);
            allSteps.Add(stepStatus.Step);

            foreach (Step step in allSteps)
            {
                StepStatus stepStatus2 = statusReportService.GetStepStatus(step);
                if (stepStatus2.Status == Status.NotStarted)
                {
                    ExecuteOneStep(stepStatus2);
                }
                else
                {
                    ExecuteTillThisStep(stepStatus2);
                }
            }


        }

        //TODO : Mozda je bespotrebno razdvojeno u posebnu metodu
        private void ExecuteOneStep(StepStatus stepStatus)
        {
            AbstractExecutor stepExecutor = CreateStepExecutor(stepStatus.Step);
            stepExecutor.Start().Wait();
        }
        /*
        private List<List<Step>> GenerateList(Step step)
        {
            List<List<Step>> allSteps = new();
            List<Step> parallelSteps = new();

            List<Step> firstLevelDependencySteps = workflowService.GetFirstLevelDependencySteps(step);

            foreach (Step s in firstLevelDependencySteps)
            {
                if (CanBeExecuted(s)) {
                    if (s.CanBeExecutedInParallel == true)
                        parallelSteps.Add(s);
                    else 
                        allSteps.Add(new List<Step>() { s });
                }
                allSteps.Add(parallelSteps);
            }

            return allSteps;

        }
        */
        /*
        private bool CanBeExecuted(Step step)
        {
            if (statusReportService.GetStepStatus(step).Status == Status.NotStarted)
                return true;

            List<Step> firstLevelDependencySteps = workflowService.GetFirstLevelDependencySteps(step);
            if(firstLevelDependencySteps.Any(s => statusReportService.GetStepStatus(s).Status != Status.Success))
            {
                return false;
            }

            return true;
        }
        */
        
        private AbstractExecutor CreateStepExecutor(Step step)
        {
            AbstractExecutor stepExecutor = ExecutorFabrique.Instance.CreateExecutor(step);
            stepExecutor.ExecutionStarted += StepExecutionStarted;
            stepExecutor.ExecutionCompleted += StepExecutionCompleted;

            return stepExecutor;
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
