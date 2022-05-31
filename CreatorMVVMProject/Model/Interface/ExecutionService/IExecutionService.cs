using CreatorMVVMProject.Model.Class.StatusReportService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Interface.ExecutionService
{
    public interface IExecutionService
    {
        void ExecuteSelectedSteps(List<StepStatus> stepsToExecute);
        void ExecuteTillThisStep(StepStatus step);

        event EventHandler? ExecutionCompleted;
        event EventHandler? ExecutionSelectedStepsStarted;
        event EventHandler? ExecutionTillThisStepStarted;
    }
}
