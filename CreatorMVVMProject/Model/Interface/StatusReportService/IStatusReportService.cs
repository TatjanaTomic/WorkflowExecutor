using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using System.Collections.Generic;

namespace CreatorMVVMProject.Model.Interface.StatusReportService
{
    public interface IStatusReportService
    {
        IList<StageStatus> Stages { get; }
        bool CanStepBeExecutedInitial(Step step);
        Status GetInitialStatus(Step step);
        StepStatus GetStepStatus(Step step);
        IList<StepStatus> GetStepStatuses(List<Step> steps);
        void SetStatusToStep(StepStatus stepStatus, Status status);
        void SetStatusToStep(Step step, Status status);
        void SetStatusMessageToStep(Step step, string message);
        void SetCanStepBeExecuted(StepStatus stepStatus);
    }
}
