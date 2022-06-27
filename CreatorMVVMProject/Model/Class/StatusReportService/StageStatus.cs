using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using System.Collections.Generic;

namespace CreatorMVVMProject.Model.Class.StatusReportService
{
    public class StageStatus
    {
        private readonly string id;
        private readonly List<StepStatus> steps = new();
        private readonly IStatusReportService statusReportService;

        public StageStatus(Stage stage, IStatusReportService statusReportService)
        {
            this.id = stage.Id;
            this.statusReportService = statusReportService;

            if (stage.Steps != null)
            {
                foreach (var step in stage.Steps)
                {
                    steps.Add(new(step, statusReportService.GetInitialStatus(step), statusReportService.CanStepBeExecutedInitial(step)));
                }
            }
        }

        public string Id { get; set; }

        public List<StepStatus> Steps => steps;

    }
}
