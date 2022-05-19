using CreatorMVVMProject.Model.Class.StatusReportService;
using ExecutionEngine.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Interface.ExecutionService
{
    public interface IExecutionService
    {
        void EnqueueSteps(List<StepStatus> stepsToExecute);
        void StartExecuteTillThisStep(StepStatus step);
    }
}
