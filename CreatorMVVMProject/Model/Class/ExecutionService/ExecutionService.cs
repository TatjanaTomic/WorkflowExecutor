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
using System.Threading;
using CreatorMVVMProject.Model.Interface.WorkflowService;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Class.Executor;

namespace CreatorMVVMProject.Model.Class.ExecutionService
{
    public class ExecutionService : IExecutionService
    {
        //private static SemaphoreSlim semaphore;

        private readonly BlockingCollection<StepStatus> stepsQueue = new();
        private readonly BlockingCollection<StepStatus> stepsQueueParallel = new();

        //CancellationTokenSource moze se iskoristiti za zaustavljanje
        private readonly CancellationTokenSource cancellationTokenSource = new();
        //Represents a thread synchronization event that, when signaled, resets automatically after releasing a single waiting thread. 
        private readonly AutoResetEvent autoResetEvent = new(true);

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
            //Task.Run(() =>
            //{
                foreach (StepStatus stepStatus in stepsToExecute)
                {
                    if (stepStatus.Step.CanBeExecutedInParallel)
                        StepsQueueParallel.Add(stepStatus);
                    else
                        StepsQueue.Add(stepStatus );

                    //statusReportService.SetStatusToStep(stepStatus, Status.Waiting);
                }

            //}).Wait();

            //autoResetEvent.Set();
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

                        //_ = autoResetEvent.WaitOne();

                        ExecuteSerialSteps(StepsQueue);
                        ExecuteParallelSteps(StepsQueueParallel);
                        
                        //autoResetEvent.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    // An InvalidOperationException means that Take() was called on a completed collection
                }
            });

            await executingSteps;
        }

        private void ExecuteSerialSteps(BlockingCollection<StepStatus> serialSteps)
        {
            while (serialSteps.Any() && !serialSteps.All(x=>x.Status==Status.Disabled))
            {
                try
                {
                    StepStatus stepStatus = serialSteps.Take();

                    if (stepStatus.Status == Status.NotStarted || stepStatus.Status == Status.Success)
                    {
                        AbstractExecutor stepExecutor = CreateStepExecutor(stepStatus.Step);
                        stepExecutor.Start().Wait();
                    }
                    else
                    {
                        serialSteps.Add(stepStatus);
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    //TODO : Sta se desava ako se ne izvrsi ?
                }

                //autoResetEvent.Set();
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
                    if (stepStatus.Status == Status.NotStarted || stepStatus.Status == Status.Success)
                    {
                        AbstractExecutor stepExecutor = CreateStepExecutor(stepStatus.Step);
                        tasks.Add(stepExecutor.Start());
                    }
                    else
                    {
                        parallelSteps.Add(stepStatus);
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    //TODO : Sta se desava ako se ne izvrsi ?
                }
            }
            Task.WaitAll(tasks.ToArray());
            //autoResetEvent.Set();
        }
        
        public async void StartExecuteTillThisStep(StepStatus stepStatus)
        {
            Task executingTask = Task.Run(() =>
            {
                List<Step> allSteps = workflowService.GetAllDependencySteps(stepStatus.Step);
                allSteps.Add(stepStatus.Step);

                List<StepStatus> stepStatuses = statusReportService.GetStepStatuses(allSteps);

                EnqueueSteps(stepStatuses);
            });

            await executingTask;
        }
        
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
