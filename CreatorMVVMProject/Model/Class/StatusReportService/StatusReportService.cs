using System.Collections.Generic;
using System.Linq;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using CreatorMVVMProject.Model.Interface.WorkflowService;

namespace CreatorMVVMProject.Model.Class.StatusReportService
{
    /// <summary>
    /// Class <c>StatusReportService</c> models a service that is responsible for setting the status of the steps.
    /// </summary>
    public class StatusReportService : IStatusReportService
    {
        private readonly IWorkflowService workflowService;

        public StatusReportService(IWorkflowService workflowService)
        {
            this.workflowService = workflowService;

            foreach (var stage in workflowService.Stages)
            {
                Stages.Add(new(stage, this));
            }
        }

        public IList<StageStatus> Stages { get; } = new List<StageStatus>();

        /// <summary>
        /// Method <c>CanStepBeExecutedInitial</c> checks if the specified step can be executed initially.
        /// If it has no dependency steps it can be executed, otherwise cannot.
        /// </summary>
        /// <param name="step">A step for which method checks if it can be executed.</param>
        /// <returns>A value that indicates if the step can be executed.</returns>
        public bool CanStepBeExecutedInitial(Step step)
        {
            return !workflowService.HasDependencySteps(step);
        }

        /// <summary>
        /// Method <c>GetInitialStatus</c> determines the initial status of the specified step.
        /// If it has dependency steps the initial status is Blocked, otherwise Ready.
        /// </summary>
        /// <param name="step">A step for which method determines the initial status.</param>
        /// <returns>Initial status of the step.</returns>
        public Status GetInitialStatus(Step step)
        {
            return workflowService.HasDependencySteps(step) ? Status.Blocked : Status.Ready;
        }

        /// <summary>
        /// Method <c>SetCanStepBeExecuted</c> sets if the specified step can be executed.
        /// If the status of the step is Blocked or InProgres it cannot be executed.
        /// If the status of the step is Obsolete and any of it's dependency steps is not executed successfully, it cannot be executed.
        /// In all other cases step can be executed.
        /// </summary>
        /// <param name="stepStatus">A step for which method sets if it can be executed.</param>
        public void SetCanStepBeExecuted(StepStatus stepStatus)
        {
            var canStepBeExecuted = true;

            if (stepStatus.Status is Status.Running)
            {
                canStepBeExecuted = false;
            }

            if (workflowService.GetAllDependencySteps(stepStatus.Step).ToList().Exists(s => GetStepStatus(s).Status != Status.Success))
            {
                canStepBeExecuted = false;
            }

            stepStatus.CanBeExecuted = canStepBeExecuted;
          
            foreach(var step in workflowService.GetReverseDependencySteps(stepStatus.Step))
            {
                SetCanStepBeExecuted(GetStepStatus(step));
            }
        }

        /// <summary>
        /// Method <c>SetStatusToStep</c> sets the new status to a passed step. 
        /// If the step changes its status to Success, the method checks all steps that depend on it. If dependency step has status Blocked 
        /// and all of its first level dependency steps are executed successfully, the dependency step changes its status to Ready.
        /// If the passed step changes its status from Success to Running, all the steps on which that step depends change their status to Obsolete. 
        /// </summary>
        /// <param name="stepStatus">A step which changes its status.</param>
        /// <param name="status">New status of a step.</param>
        public void SetStatusToStep(StepStatus stepStatus, Status status)
        {
            Status oldStatus = stepStatus.Status;
            stepStatus.Status = status;

            if (status == Status.Success)
            {
                IList<StepStatus> reverseDependencyStepStatuses = GetStepStatuses(workflowService.GetReverseDependencySteps(stepStatus.Step).ToList());

                foreach (StepStatus dependencyStepStatus in reverseDependencyStepStatuses)
                {
                    IList<StepStatus> firstLevelDependencySteps = GetStepStatuses(workflowService.GetFirstLevelDependencySteps(dependencyStepStatus.Step).ToList());

                    if (dependencyStepStatus.Status == Status.Blocked && firstLevelDependencySteps.All(s => s.Status == Status.Success))
                    {
                        SetStatusToStep(dependencyStepStatus, Status.Ready);
                    }
                }
            }

            //if (oldStatus.Equals(Status.Success) && status.Equals(Status.Running))
            if (oldStatus.Equals(Status.Success))
            {
                IList<Step> obsoletedSteps = workflowService.GetReverseDependencySteps(stepStatus.Step);
                foreach (Step step in obsoletedSteps)
                {
                    if(GetStepStatus(step).Status is not Status.Ready and not Status.Blocked)
                    {
                        SetStatusToStep(step, Status.Obsolete);
                    }
                }
            }

            SetCanStepBeExecuted(stepStatus);
        }

        public void SetStatusToStep(Step step, Status status)
        {
            StepStatus stepStatus = GetStepStatus(step);
            SetStatusToStep(stepStatus, status);
        }

        public void SetStatusMessageToStep(Step step, string message)
        {
            StepStatus stepStatus = GetStepStatus(step);
            stepStatus.StatusMessage = message;
        }

        public StepStatus GetStepStatus(Step step)
        {
            return Stages.SelectMany(stage => stage.Steps).First(s => s.Step.Id == step.Id);
        }

        public IList<StepStatus> GetStepStatuses(List<Step> steps)
        {
            IList<StepStatus> stepStatuses = new List<StepStatus>();
            foreach (Step step in steps)
            {
                stepStatuses.Add(GetStepStatus(step));
            }
            return stepStatuses;
        }
    }
}
