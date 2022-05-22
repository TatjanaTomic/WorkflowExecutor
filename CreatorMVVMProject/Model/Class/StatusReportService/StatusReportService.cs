using CreatorMVVMProject.Model.Class.WorkflowService;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using CreatorMVVMProject.Model.Interface.WorkflowService;
using ExecutionEngine.Xml;
using System.Collections.Generic;
using System.Linq;

namespace CreatorMVVMProject.Model.Class.StatusReportService
{
    public class StatusReportService : IStatusReportService
    {
        private readonly IWorkflowService workflowService;
        private readonly IList<StageStatus> stages = new List<StageStatus>();

        public StatusReportService(IWorkflowService workflowService)
        {
            this.workflowService = workflowService;

            foreach(var stage in workflowService.Stages)
            {
                stages.Add(new(stage, this));
            }
        }

        public IList<StageStatus> Stages => this.stages;

        public Status GetInitialStatus(Step step)
        {
            if (workflowService.HasDependencySteps(step))
                return Status.Disabled;
            else
                return Status.NotStarted;
        }

        public void SetStatusToStep(Step step, Status status)
        {
            StepStatus stepStatus = GetStepStatus(step);
            SetStatusToStep(stepStatus, status);
        }

        public void SetStatusToStep(StepStatus stepStatus, Status status) 
        {
            Status oldStatus = stepStatus.Status;
            stepStatus.Status = status; //promijeni odmah status na novi

            if(status == Status.Success)
            {
                //TODO : Pogresna logika, stepStatus2 prelazi u NotStarted samo ako su svi njegovi zavisni izvrseni !!!
                List<Step> reverseDependencySteps = workflowService.GetReverseDependencySteps(stepStatus.Step);
                foreach (Step step in reverseDependencySteps)
                {
                    //StepStatus stepStatus2 = GetStepStatus(step);
                    List<StepStatus> firstLevelDependencySteps = GetStepStatuses(workflowService.GetFirstLevelDependencySteps(step));
                    
                    //TODO : Dovrsi prelaz u NotStarted status
                    //if(firstLevelDependencySteps.All(s => s.))
                    //if (stepStatus2.Status == Status.Disabled)
                    //    stepStatus2.Status = Status.NotStarted;

                    if(firstLevelDependencySteps.All(s => s.Status == Status.Success))
                    {
                        StepStatus stepStatus2 = GetStepStatus(step);
                        stepStatus2.Status = Status.NotStarted;
                    }
                }
            }

            if (oldStatus.Equals(Status.Success) && status.Equals(Status.InProgress))
            {
                List<Step> obsoletedSteps = workflowService.GetAllDependencySteps(stepStatus.Step);
                foreach(Step step in obsoletedSteps)
                {
                    StepStatus stepStatus2 = GetStepStatus(step);
                    stepStatus2.Status = Status.Obsolete;
                }
            }

        }

        public StepStatus GetStepStatus(Step step)
        {
            return stages.SelectMany(stage => stage.Steps).Where(s => s.Step.Id == step.Id).First();
        }

        public List<StepStatus> GetStepStatuses(List<Step> steps)
        {
            List<StepStatus> stepStatuses = new();
            foreach (Step step in steps)
            {
                stepStatuses.Add(GetStepStatus(step));
            }
            return stepStatuses;
        }
    }
}
