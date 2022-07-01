using System;
using System.Collections.Generic;
using CreatorMVVMProject.Model.Class.StatusReportService;

namespace CreatorMVVMProject.Model.Interface.ExecutionService
{
    public interface IExecutionService
    {
        event EventHandler? ExecutionCompleted;
        event EventHandler? ExecutionSelectedStepsStarted;
        event EventHandler? ExecutionTillThisStepStarted;

        void ExecuteTillThisStep(StepStatus stepStatus);
        void ExecuteSelectedSteps(List<StepStatus> stepsToExecute);

    }
}
