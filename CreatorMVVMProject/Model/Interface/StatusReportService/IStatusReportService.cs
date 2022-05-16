using CreatorMVVMProject.Model.Class.StatusReportService;
using ExecutionEngine.Step;
using ExecutionEngine.Xml;
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
        Status GetInitialStatus(Step step);
    }
}
