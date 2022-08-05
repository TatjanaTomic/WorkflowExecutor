using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Class.StepExecutor;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Interface.ExecutionService;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using CreatorMVVMProject.Model.Interface.WorkflowService;

namespace CreatorMVVMProject.Model.Class.ExecutionService
{
    /// <summary>
    /// Class <c>ExecutionService</c> models a service that is responsible for executing serial and parallel steps.
    /// </summary>
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
        public event EventHandler? ExecutionSelectedStepsStarted;
        public event EventHandler? ExecutionTillThisStepStarted;

        /// <summary>
        /// Serial steps queue
        /// </summary>
        private BlockingCollection<StepStatus> StepsQueue { get; } = new();

        /// <summary>
        /// Parallel steps queue
        /// </summary>
        private BlockingCollection<StepStatus> StepsQueueParallel { get; } = new();

        /// <summary>
        /// Method <c>StartExecution</c> runs long running Task that executes Steps in Queues.
        /// It calls methods <c>ExecuteSerialSteps</c> and <c>ExecuteParallelSteps</c> and then waits for the auto reset event to be set.
        /// When cancellation is requested, it raises event ExecutionCompleted.
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
                            ExecutionCompleted?.Invoke(this, EventArgs.Empty);
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
        /// Method <c>ExecuteSelectedSteps</c> adds the passed steps to the queues and raises execution selected steps started.
        /// </summary>
        /// <param name="stepsToExecute">A list of steps to be executed.</param>
        public void ExecuteSelectedSteps(List<StepStatus> stepsToExecute)
        {
            Task.Run(() =>
            {
                EnqueueSteps(stepsToExecute);

                ExecutionSelectedStepsStarted?.Invoke(this, EventArgs.Empty);
            });
        }

        /// <summary>
        /// Method <c>ExecuteTillThisStep</c> takes a step to be executed. It builds a list of all dependency steps of the passed step (containing that step)
        /// and calls method <c>EnqueueSteps</c> passing it that list.
        /// The method raises execution till this step started.
        /// </summary>
        /// <param name="stepStatus">A step to be executed.</param>
        public void ExecuteTillThisStep(StepStatus stepStatus)
        {
            Task.Run(() =>
            {
                IList<Step> allSteps = workflowService.GetAllDependencySteps(stepStatus.Step);
                allSteps.Add(stepStatus.Step);

                IList<StepStatus> stepStatuses = statusReportService.GetStepStatuses(allSteps.ToList());

                EnqueueSteps(stepStatuses.ToList());

                ExecutionTillThisStepStarted?.Invoke(this, EventArgs.Empty);
            });

        }

        /// <summary>
        /// Method <c>EnqueueSteps</c> takes a list of steps and checks for each step if it can be executed in parallel or not. 
        /// If a step can be executed in parallel, the method adds it to parallel steps queue. Otherwise, the step is added to the serial steps queue.
        /// At the end, it sets auto reset event.
        /// </summary>
        /// <param name="stepsToExecute">List of steps that need to bee executed.</param>
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
        /// Method <c>ExecuteParallelSteps</c> starts all serial steps that can be executed.
        /// The method is performed while there is any step in serial steps queue and not all steps have status Blocked.
        /// It takes steps from the queue one by one and checks if step's status is Blocked, if so the step is added at the end of the queue.
        /// Otherwise, an executor for that step is created and the step is started. The method waits for the step to complete execution.
        /// The method raises execution completed event when there is no serial nor parallel steps to be executed.
        /// </summary>
        private void ExecuteSerialSteps()
        {
            while (StepsQueue.Any() && !StepsQueue.All(s => s.Status == Status.Blocked))
            {
                try
                {
                    StepStatus stepStatus = StepsQueue.Take();

                    if (stepStatus.Status == Status.Blocked)
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
                    ExecutionCompleted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    autoResetEvent.Set();
                }
            }

        }

        /// <summary>
        /// Method <c>ExecuteParallelSteps</c> creates a list of tasks and starts all the paralle steps that can be executed.
        /// The method is performed while there is any step in parallel steps queue and not all steps have status Blocked.
        /// It takes steps from the queue one by one and checks if step's status is Blocked, if so the step is added at the end of the queue.
        /// Otherwise, an executor for that step and new task are created.
        /// The method waits for all the steps to complete execution.
        /// The method raises execution completed event when there is no serial nor parallel steps to be executed.
        /// When queues of serial and parallel steps are empty, the execution is finished and the method raises an event. Otherwise, it sets auto reset event. 
        /// </summary>
        private void ExecuteParallelSteps()
        {
            List<Task> tasks = new();
            while (StepsQueueParallel.Any() && !StepsQueueParallel.All(s => s.Status == Status.Blocked))
            {
                StepStatus stepStatus = StepsQueueParallel.Take();
                try
                {
                    if (stepStatus.Status == Status.Blocked)
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
                ExecutionCompleted?.Invoke(this, EventArgs.Empty);
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
            statusReportService.SetStatusToStep(e, Status.Running);
        }

        private void StepExecutionCompleted(object? _, ExecutionCompletedEventArgs args)
        {
            statusReportService.SetStatusToStep(args.Step, args.IsSuccessful ? Status.Success : Status.Failed);
            statusReportService.SetStatusMessageToStep(args.Step, args.Message);

            // If any of the steps failes, execution stops
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
