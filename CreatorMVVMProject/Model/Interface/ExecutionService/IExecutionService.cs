using CreatorMVVMProject.Model.Class.StatusReportService;
using System;
using System.Collections.Generic;

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
