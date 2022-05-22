using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Interface.StatusReportService
{
    public interface IStatusReportService
    {
        IList<StageStatus> Stages
        {
            get;
        }
        void SetStatusToStep(StepStatus stepStatus, Status status);
        void SetStatusToStep(Step e, Status inProgress);
        Status GetInitialStatus(Step step);
        StepStatus GetStepStatus(Step step);
    }
}
