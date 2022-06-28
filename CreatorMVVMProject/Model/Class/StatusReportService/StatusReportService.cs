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

            foreach(var stage in workflowService.Stages)
            {
                Stages.Add(new(stage, this));
            }
        }

        public IList<StageStatus> Stages { get; } = new List<StageStatus>();

        public Status GetInitialStatus(Step step)
        {
            if (workflowService.HasDependencySteps(step))
            {
                return Status.Disabled;
            }
            
            return Status.NotStarted;
        }

        public bool CanStepBeExecutedInitial(Step step)
        {
            return !workflowService.HasDependencySteps(step);
        }

        public void SetCanStepBeExecuted(StepStatus stepStatus)
        {
            var canStepBeExecuted = true;

            if (stepStatus.Status == Status.Disabled || stepStatus.Status == Status.InProgress)
            {
                canStepBeExecuted = false;
            }

            if (stepStatus.Status == Status.Obsolete && workflowService.GetAllDependencySteps(stepStatus.Step).ToList().Exists(s => GetStepStatus(s).Status != Status.Success))
            {
                canStepBeExecuted = false;
            }

            stepStatus.CanBeExecuted = canStepBeExecuted;
        }

        public void SetStatusToStep(StepStatus stepStatus, Status status) 
        {
            Status oldStatus = stepStatus.Status;
            stepStatus.Status = status; //promijeni odmah status na novi

            if(status == Status.Success)
            {
                IList<StepStatus> reverseDependencySteps = GetStepStatuses(workflowService.GetReverseDependencySteps(stepStatus.Step).ToList());

                foreach (StepStatus dependencyStepStatus in reverseDependencySteps)
                {
                    IList<StepStatus> firstLevelDependencySteps = GetStepStatuses(workflowService.GetFirstLevelDependencySteps(dependencyStepStatus.Step).ToList());  

                    if(dependencyStepStatus.Status == Status.Disabled && firstLevelDependencySteps.All(s => s.Status == Status.Success) )
                    {
                        SetStatusToStep(dependencyStepStatus, Status.NotStarted);
                    }
                }
            }

            if (oldStatus.Equals(Status.Success) && status.Equals(Status.InProgress))
            {
                IList<Step> obsoletedSteps = workflowService.GetAllDependencySteps(stepStatus.Step);
                foreach(Step step in obsoletedSteps)
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
