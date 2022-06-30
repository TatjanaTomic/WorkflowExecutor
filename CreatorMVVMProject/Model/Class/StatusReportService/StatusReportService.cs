using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using CreatorMVVMProject.Model.Interface.WorkflowService;
using System.Collections.Generic;
using System.Linq;

namespace CreatorMVVMProject.Model.Class.StatusReportService
{
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
        /// <param name="step">Step for which method checks if it can be executed.</param>
        /// <returns>Value that indicates if the step can be executed.</returns>
        public bool CanStepBeExecutedInitial(Step step)
        {
            return !workflowService.HasDependencySteps(step);
        }

        /// <summary>
        /// Method <c>GetInitialStatus</c> determines the initial status of the specified step.
        /// If it has dependency steps the initial status is Disabled, otherwise NotStarted.
        /// </summary>
        /// <param name="step">Step for which method determines initial status.</param>
        /// <returns>Initial status of the step.</returns>
        public Status GetInitialStatus(Step step)
        {
            return workflowService.HasDependencySteps(step) ? Status.Disabled : Status.NotStarted;
        }

        /// <summary>
        /// Method <c>SetCanStepBeExecuted</c> sets if the specified step can be executed.
        /// If status of step is Disabled or InProgres it cannot be executed.
        /// If status of step is Obsolete and any of it's dependency steps is not executed successfully, it cannot be executed.
        /// In all other cases step can be executed.
        /// </summary>
        /// <param name="stepStatus">Step for which method sets if it can be executed.</param>
        public void SetCanStepBeExecuted(StepStatus stepStatus)
        {
            var canStepBeExecuted = true;

            if (stepStatus.Status is Status.Disabled or Status.InProgress)
            {
                canStepBeExecuted = false;
            }

            if (stepStatus.Status is Status.Obsolete && workflowService.GetAllDependencySteps(stepStatus.Step).ToList().Exists(s => GetStepStatus(s).Status != Status.Success))
            {
                canStepBeExecuted = false;
            }

            stepStatus.CanBeExecuted = canStepBeExecuted;
        }

        //TODO: Finish this
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stepStatus"></param>
        /// <param name="status"></param>
        public void SetStatusToStep(StepStatus stepStatus, Status status)
        {
            Status oldStatus = stepStatus.Status;
            stepStatus.Status = status; //promijeni odmah status na novi

            if (status == Status.Success)
            {
                IList<StepStatus> reverseDependencySteps = GetStepStatuses(workflowService.GetReverseDependencySteps(stepStatus.Step).ToList());

                foreach (StepStatus dependencyStepStatus in reverseDependencySteps)
                {
                    IList<StepStatus> firstLevelDependencySteps = GetStepStatuses(workflowService.GetFirstLevelDependencySteps(dependencyStepStatus.Step).ToList());

                    if (dependencyStepStatus.Status == Status.Disabled && firstLevelDependencySteps.All(s => s.Status == Status.Success))
                    {
                        SetStatusToStep(dependencyStepStatus, Status.NotStarted);
                    }
                }
            }

            if (oldStatus.Equals(Status.Success) && status.Equals(Status.InProgress))
            {
                IList<Step> obsoletedSteps = workflowService.GetAllDependencySteps(stepStatus.Step);
                foreach (Step step in obsoletedSteps)
                {
                    SetStatusToStep(step, Status.Obsolete);
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
