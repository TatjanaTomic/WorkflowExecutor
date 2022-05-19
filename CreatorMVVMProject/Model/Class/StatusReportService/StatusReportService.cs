using CreatorMVVMProject.Model.Class.WorkflowService;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using CreatorMVVMProject.Model.Interface.WorkflowService;
using ExecutionEngine.Step;
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

        public IList<StageStatus> Stages
        {
            get
            {
                return this.stages;
            }
        }

        public Status GetInitialStatus(Step step)
        {
            if (workflowService.HasDependencySteps(step))
                return Status.Disabled;
            else
                return Status.NotStarted;
        }

        public void SetStatusToStep(Step step, Status status)
        {
            StepStatus stepStatus = stages.SelectMany(stage => stage.Steps).Where(s => s.Step.Id == step.Id).First();
            SetStatusToStep(stepStatus, status);
        }

        public void SetStatusToStep(StepStatus stepStatus, Status status) 
        {
            Status oldStatus = stepStatus.Status;
            stepStatus.Status = status; //promijeni odmah status na novi

            if(status == Status.Success)
            {
                List<Step> notStartedSteps = workflowService.GetReverseDependencySteps(stepStatus.Step);
                foreach (Step step in notStartedSteps)
                {
                    StepStatus stepStatus2 = stages.SelectMany(stage => stage.Steps).Where(s => s.Step.Id == step.Id).First();
                    if (stepStatus2.Status == Status.Disabled)
                        stepStatus2.Status = Status.NotStarted;
                }
            }

            if (oldStatus.Equals(Status.Success) && status.Equals(Status.InProgress))
            {
                //TODO : Obsoleted
            }

            // TODO : Dovrsi

        

        }

        public StepStatus GetStepStatus(Step step)
        {
            return stages.SelectMany(stage => stage.Steps).Where(s => s.Step.Id == step.Id).First();
        }
    }
}
