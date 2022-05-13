using CreatorMVVMProject.Model.Interface.WorkflowService;
using System.Collections.Generic;
using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using ExecutionEngine.Xml;

namespace CreatorMVVMProject.Model.Class.Main
{
    public class MainModel
    {
        private readonly IStatusReportService statusReportService;
        private readonly IWorkflowService workflowService;

        public MainModel(IStatusReportService statusReportService, IWorkflowService workflowService)
        {
            this.statusReportService = statusReportService;
            this.workflowService = workflowService;
        }

        public IList<StageStatus> Stages
        {
            get => this.statusReportService.Stages;
        }
    }
}
