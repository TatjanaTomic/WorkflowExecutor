using System;
using System.Collections.Generic;
using CreatorMVVMProject.Model.Class.StatusReportService;

namespace CreatorMVVMProject.Model.Interface.ExecutionService
{
    public interface IExecutionService
    {
        event EventHandler<string>? ExecutionCompleted;
        event EventHandler<string>? ExecutionSelectedStepsStarted;
        event EventHandler<string>? ExecutionTillThisStepStarted;

        void ExecuteTillThisStep(StepStatus stepStatus);
        void ExecuteSelectedSteps(List<StepStatus> stepsToExecute);

    }
}
