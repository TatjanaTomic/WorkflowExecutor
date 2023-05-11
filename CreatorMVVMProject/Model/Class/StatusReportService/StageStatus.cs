using System.Collections.Generic;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Interface.StatusReportService;

namespace CreatorMVVMProject.Model.Class.StatusReportService;

public class StageStatus
{
    private readonly IStatusReportService statusReportService;

    public StageStatus(Stage stage, IStatusReportService statusReportService)
    {
        Id = stage.Id;
        this.statusReportService = statusReportService;

        if (stage.Steps != null)
        {
            foreach (var step in stage.Steps)
            {
                Steps.Add(new(step, statusReportService.GetInitialStatus(step), statusReportService.CanStepBeExecutedInitial(step)));
            }
        }
    }

    public string Id { get; }

    public List<StepStatus> Steps { get; } = new();

}
