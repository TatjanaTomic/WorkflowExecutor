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

        public void SetStatusToStep(StepStatus stepStatus, Status status) 
        {
            stepStatus.Status = status;

            StageStatus? stageStatus = this.Stages.SingleOrDefault(st => st.Steps.Contains(stepStatus));
            int indexOfStage = this.Stages.IndexOf(stageStatus); 
            int indexOfStep = stageStatus.Steps.IndexOf(stepStatus);

            stepStatus = stageStatus.Steps[indexOfStep];
            StepStatus dependencyStepStatus;
            foreach (Dependency dependencyStep in stepStatus.Step.Dependencies)
            {
                dependencyStepStatus = stageStatus.Steps.SingleOrDefault(st => st.Step.Dependencies.Contains(dependencyStep));
                dependencyStepStatus.Status = Status.Obsolete;
            }

        }

        /*
        public void SetStatusToProcess(ProcessStatus process, Status status)
        {
            process.Status = Status.InProgress;
            StageStatus? stage = this.stages.SingleOrDefault(st => st.Processes.Contains(process));
            int indexOfProcess = stage.Processes.IndexOf(process);
            int indexOfStage = this.stages.IndexOf(stage);
            int j = indexOfProcess + 1;
            for (int i = indexOfStage; i < this.stages.Count; i++)
            {
                for (; j<this.stages[i].Processes.Count; j++)
                {
                    this.stages[i].Processes[j].Status = Status.Obsolete;
                }
                j=0;
            }
        }*/
    }
}
