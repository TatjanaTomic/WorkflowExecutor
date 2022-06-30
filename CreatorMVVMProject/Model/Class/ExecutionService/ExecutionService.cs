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
        private readonly IStatusReportService statusReportService;
        private readonly IWorkflowService workflowService;
        private readonly AutoResetEvent autoResetEvent = new(false);
        private readonly CancellationTokenSource cancellationTokenSource = new();

        public ExecutionService(IStatusReportService statusReportService, IWorkflowService workflowService)
        {
            this.statusReportService = statusReportService;
            this.workflowService = workflowService;

            _ = StartExecution();
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

        private BlockingCollection<StepStatus> StepsQueue { get; } = new();

        private BlockingCollection<StepStatus> StepsQueueParallel { get; } = new();

        /// <summary>
        /// Method <c>StartExecution</c> runs long running Task that executes Steps in Queues.
        /// 
        /// </summary>
        /// <returns></returns>
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

                        ExecuteSerialSteps();
                        ExecuteParallelSteps();

                        autoResetEvent.WaitOne();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            });

            await executingSteps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stepsToExecute"></param>
        public void ExecuteSelectedSteps(List<StepStatus> stepsToExecute)
        {
            Task.Run(() =>
            {
                EnqueueSteps(stepsToExecute);

                OnExecutionSelectedStepsStarted();
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stepStatus"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stepsToExecute"></param>
        private void EnqueueSteps(List<StepStatus> stepsToExecute)
        {
            foreach (StepStatus stepStatus in stepsToExecute)
            {
                if (stepStatus.Step.CanBeExecutedInParallel)
                {
                    StepsQueueParallel.Add(stepStatus);
                }
                else
                {
                    StepsQueue.Add(stepStatus);
                }

                stepStatus.CanBeExecuted = false;
            }

            autoResetEvent.Set();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteSerialSteps()
        {
            while (StepsQueue.Any() && !StepsQueue.All(s => s.Status == Status.Disabled))
            {
                try
                {
                    StepStatus stepStatus = StepsQueue.Take();

                    if (stepStatus.Status == Status.Disabled)
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
                {
                    OnExecutionCompleted();
                }
                else
                {
                    autoResetEvent.Set();
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteParallelSteps()
        {
            List<Task> tasks = new();
            while (StepsQueueParallel.Any() && !StepsQueueParallel.All(s => s.Status == Status.Disabled))
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
            {
                OnExecutionCompleted();
            }
            else
            {
                autoResetEvent.Set();
            }
        }

        private AbstractExecutor CreateStepExecutor(Step step)
        {
            AbstractExecutor stepExecutor = StepExecutorFabrique.CreateExecutor(step);
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

            //If any of steps failed, execution stops
            if (!args.IsSuccessful)
            {
                ClearQueues();
                cancellationTokenSource.Cancel();
            }
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
