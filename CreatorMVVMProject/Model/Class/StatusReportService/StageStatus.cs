using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using System.Collections.Generic;

namespace CreatorMVVMProject.Model.Class.StatusReportService
{
    public class StageStatus
    {
        private string id = string.Empty;
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

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public List<StepStatus> Steps
        {
            get => this.steps;
        }

    }
}
