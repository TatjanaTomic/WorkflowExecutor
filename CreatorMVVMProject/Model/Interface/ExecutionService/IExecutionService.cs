using CreatorMVVMProject.Model.Class.StatusReportService;
using System;
using System.Collections.Generic;

namespace CreatorMVVMProject.Model.Interface.ExecutionService
{
    public interface IExecutionService
    {
        void ExecuteSelectedSteps(List<StepStatus> stepsToExecute);
        void ExecuteTillThisStep(StepStatus stepStatus);

        event EventHandler? ExecutionCompleted;
        event EventHandler? ExecutionSelectedStepsStarted;
        event EventHandler? ExecutionTillThisStepStarted;
    }
}
