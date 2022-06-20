using CreatorMVVMProject.Model.Interface.ExecutionService;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly AutoResetEvent autoResetEvent = new(false);

        private readonly IStatusReportService statusReportService;
        private readonly IWorkflowService workflowService;

        public ExecutionService(IStatusReportService statusReportService, IWorkflowService workflowService)
        {
            this.statusReportService = statusReportService;
            this.workflowService = workflowService;

            _ = StartExecution();
        }
        
        private void EnqueueSteps(List<StepStatus> stepsToExecute)
        {
            foreach (StepStatus stepStatus in stepsToExecute)
            {
                if (stepStatus.Step.CanBeExecutedInParallel)
                    StepsQueueParallel.Add(stepStatus);
                else
                    StepsQueue.Add(stepStatus );

                stepStatus.CanBeExecuted = false;
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
                        if (cancellationTokenSource.IsCancellationRequested)
                        {

                            OnExecutionCompleted();
                        }

                        ExecuteParallelSteps();
                        ExecuteSerialSteps();
                        
                        autoResetEvent.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            });

            await executingSteps;
        }


        private void ExecuteSerialSteps()
        {
            while (StepsQueue.Any() && !StepsQueue.All(x => x.Status == Status.Disabled))
            {
                try
                {
                    StepStatus stepStatus = StepsQueue.Take();

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
                }

                if (StepsQueue.Count == 0 && StepsQueueParallel.Count == 0)
                    OnExecutionCompleted();
                else
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
                }
            }
            Task.WaitAll(tasks.ToArray());

            if (StepsQueue.Count == 0 && StepsQueueParallel.Count == 0)
                OnExecutionCompleted();
            else
                autoResetEvent.Set();
        }
        
        public void ExecuteSelectedSteps(List<StepStatus> stepsToExecute)
        {
            Task.Run(() =>
            {
                EnqueueSteps(stepsToExecute);

                OnExecutionSelectedStepsStarted();
            });

        }

        public void ExecuteTillThisStep(StepStatus stepStatus)
        {
            Task.Run(() =>
            {
                IList<Step> allSteps = workflowService.GetAllDependencySteps(stepStatus.Step);
                allSteps.Add(stepStatus.Step);

                IList<StepStatus> stepStatuses = statusReportService.GetStepStatuses(allSteps.ToList());

                EnqueueSteps(stepStatuses.ToList());

                OnExecutionTillThisStepStarted();
            });

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
            statusReportService.SetStatusToStep(args.Step, args.IsSuccessful ? Status.Success : Status.Failed);
            statusReportService.SetStatusMessageToStep(args.Step, args.Message);

            //TODO: Ako bilo koji step padne, zaustavljam izvrsavanje
            if(!args.IsSuccessful)
            {
                //TODO: Bug - za svaki step koji ce se 'ocistiti' treba promijeniti parametar canBeExecuted
                ClearQueues();
                cancellationTokenSource.Cancel();
            }
        }

        public event EventHandler? ExecutionCompleted;
        protected virtual void OnExecutionCompleted()
        {
            ExecutionCompleted?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? ExecutionSelectedStepsStarted;
        protected virtual void OnExecutionSelectedStepsStarted()
        {
            ExecutionSelectedStepsStarted?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? ExecutionTillThisStepStarted;
        protected virtual void OnExecutionTillThisStepStarted()
        {
            ExecutionTillThisStepStarted?.Invoke(this, EventArgs.Empty);
        }

        private void ClearQueues()
        {
            while (StepsQueue.TryTake(out var stepStatus))
            {
                statusReportService.SetCanStepBeExecuted(stepStatus);
            }

            while (StepsQueueParallel.TryTake(out var stepStatus))
            {
                statusReportService.SetCanStepBeExecuted(stepStatus);
            }
        }
    }
}
