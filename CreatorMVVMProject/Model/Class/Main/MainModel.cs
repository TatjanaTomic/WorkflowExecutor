using CreatorMVVMProject.Model.Interface.WorkflowService;
using System.Collections.Generic;
using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using ExecutionEngine.Xml;
using CreatorMVVMProject.Model.Interface.ExecutionService;

namespace CreatorMVVMProject.Model.Class.Main
{
    public class MainModel
    {
        private readonly IStatusReportService statusReportService;
        private readonly IWorkflowService workflowService;
        private readonly IExecutionService executionService;

        public MainModel(IStatusReportService statusReportService, IWorkflowService workflowService, IExecutionService executionService)
        {
            this.statusReportService = statusReportService;
            this.workflowService = workflowService;
            this.executionService = executionService;
        }

        public IList<StageStatus> Stages
        {
            get => this.statusReportService.Stages;
        }

        public void AddStepsToExecution(List<StepStatus> steps)
        {
            executionService.EnqueueSteps(steps);
        }
    }
}
