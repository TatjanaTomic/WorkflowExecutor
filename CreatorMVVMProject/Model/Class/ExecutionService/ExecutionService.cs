using CreatorMVVMProject.Model.Interface.ExecutionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using CreatorMVVMProject.Model.Class.StatusReportService;
using System.Threading;
using CreatorMVVMProject.Model.Interface.WorkflowService;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Class.StepExecutor;

namespace CreatorMVVMProject.Model.Class.ExecutionService
{
    public class ExecutionService : IExecutionService
    {
        private readonly BlockingCollection<StepStatus> stepsQueue = new();
        private readonly BlockingCollection<StepStatus> stepsQueueParallel = new();

        //CancellationTokenSource moze se iskoristiti za zaustavljanje
        //private readonly CancellationTokenSource cancellationTokenSource = new();
        
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
        
        //TODO : Kako da napravim Event OnExecutionFinished, da znam da su se svi proslijedjeni stepovi izvrsili ?
        public void EnqueueSteps(List<StepStatus> stepsToExecute)
        {
            foreach (StepStatus stepStatus in stepsToExecute)
            {
                if (stepStatus.Step.CanBeExecutedInParallel)
                    StepsQueueParallel.Add(stepStatus);
                else
                    StepsQueue.Add(stepStatus );

                //statusReportService.SetStatusToStep(stepStatus, Status.Waiting);
            }

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

                        ExecuteParallelSteps();
                        ExecuteSerialSteps();

                        autoResetEvent.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    //TODO : Sta se desava ako dodje do exceptiona ? Gasim App ?
                    Console.WriteLine(e.Message);

                    // An InvalidOperationException means that Take() was called on a completed collection
                }
            });

            await executingSteps;
        }

        private void ExecuteSerialSteps()
        {
            while (StepsQueue.Any() && !StepsQueue.All(x => x.Status==Status.Disabled))
            {
                try
                {
                    StepStatus stepStatus = StepsQueue.Take();

                    /*
                    if (stepStatus.Status == Status.NotStarted || stepStatus.Status == Status.Success)
                    {
                        AbstractExecutor stepExecutor = CreateStepExecutor(stepStatus.Step);
                        stepExecutor.Start().Wait();
                    }
                    else
                    {
                        StepsQueue.Add(stepStatus);
                        continue;
                    }
                    */

                    if(stepStatus.Status == Status.Disabled)
                    {
                        StepsQueue.Add(stepStatus);
                        continue;
                    }
                    
                    AbstractExecutor stepExecutor = CreateStepExecutor(stepStatus.Step);
                    stepExecutor.Start().Wait();
                    

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    //TODO : Sta se desava ako se ne izvrsi ?
                }

                autoResetEvent.Set();
            }

        }

        private void ExecuteParallelSteps()
        {
            List<Task> tasks = new();
            while (StepsQueueParallel.Any() && !StepsQueueParallel.All(x => x.Status == Status.Disabled))
            {
                StepStatus stepStatus = StepsQueueParallel.Take();
                try
                {
                    /*
                    if (stepStatus.Status == Status.NotStarted || stepStatus.Status == Status.Success)
                    {
                        AbstractExecutor stepExecutor = CreateStepExecutor(stepStatus.Step);
                        tasks.Add(stepExecutor.Start());
                    }
                    else
                    {
                        StepsQueueParallel.Add(stepStatus);
                        continue;
                    }
                    */

                    if (stepStatus.Status == Status.Disabled)
                    {
                        StepsQueueParallel.Add(stepStatus);
                        continue;
                    }

                    AbstractExecutor stepExecutor = CreateStepExecutor(stepStatus.Step);
                    tasks.Add(stepExecutor.Start());

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    //TODO : Sta se desava ako se ne izvrsi ?
                }
            }
            Task.WaitAll(tasks.ToArray());
            autoResetEvent.Set();
        }
        
        public void StartExecuteTillThisStep(StepStatus stepStatus)
        {
            Task.Run(() =>
            {
                List<Step> allSteps = workflowService.GetAllDependencySteps(stepStatus.Step);
                allSteps.Add(stepStatus.Step);

                List<StepStatus> stepStatuses = statusReportService.GetStepStatuses(allSteps);

                EnqueueSteps(stepStatuses);
            }).Wait();

        }

        private BlockingCollection<StepStatus> StepsQueue
        {
            get { return stepsQueue; }
        }

        private BlockingCollection<StepStatus> StepsQueueParallel
        {
            get { return stepsQueueParallel; }
        }

        private AbstractExecutor CreateStepExecutor(Step step)
        {
            AbstractExecutor stepExecutor = StepExecutorFabrique.Instance.CreateExecutor(step);
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
            {
                statusReportService.SetStatusToStep(args.Step, Status.Success);
            }
            else
            {
                statusReportService.SetStatusToStep(args.Step, Status.Failed);
            }

            statusReportService.SetStatusMessageToStep(args.Step, args.Message);
        }
  
    }
}
