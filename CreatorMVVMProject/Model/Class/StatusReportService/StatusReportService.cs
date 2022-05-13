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
        private IList<StageStatus> stages = new List<StageStatus>();

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

        public IList<Step> GetFirstLevelDependencySteps(Step step)
        {
            return this.workflowService.GetFirstLevelDependencySteps(step);
        }

        public IList<Step> GetAllDependencySteps(Step step)
        {
            return this.workflowService.GetAllDependencySteps(step);
        }

        public void SetStatusToStep(StepStatus stepStatus, Status status) 
        {
            stepStatus.Status = status;

            

            //StageStatus? stageStatus = this.Stages.SingleOrDefault(st => st.Steps.Contains(stepStatus));
            //int indexOfStage = this.Stages.IndexOf(stageStatus); 
            //int indexOfStep = stageStatus.Steps.IndexOf(stepStatus);

            //stepStatus = stageStatus.Steps[indexOfStep];
            //StepStatus dependencyStepStatus;
            //foreach (Dependency dependencyStep in stepStatus.Step.Dependencies)
            //{
            //    dependencyStepStatus = stageStatus.Steps.SingleOrDefault(st => st.Step.Dependencies.Contains(dependencyStep));
            //    dependencyStepStatus.Status = Status.Obsolete;
            //}

        }
    }
}
