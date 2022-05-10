using CreatorMVVMProject.Model.Interface.WorkflowService;
using System.Collections.Generic;
using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Interface.StatusReportService;

namespace CreatorMVVMProject.Model.Class.Main
{
    public class MainModel
    {
        private readonly IStatusReportService statusReportService;
        public IList<StageStatus> Stages
        {
            get => this.statusReportService.Stages;
        }
        public MainModel(IStatusReportService statusReportService)
        {
            this.statusReportService = statusReportService;
        }
    }
}
