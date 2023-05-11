using System;
using System.Collections.Generic;
using CreatorMVVMProject.Model.Class.ExecutionService;
using CreatorMVVMProject.Model.Class.StatusReportService;

namespace CreatorMVVMProject.Model.Interface.ExecutionService;

public interface IExecutionService
{
    event EventHandler<ExecutionEventArgs>? ExecutionCompleted;
    event EventHandler<ExecutionEventArgs>? ExecutionSelectedStepsStarted;
    event EventHandler<ExecutionEventArgs>? ExecutionTillThisStepStarted;

    void ExecuteTillThisStep(StepStatus stepStatus);
    void ExecuteSelectedSteps(List<StepStatus> stepsToExecute);

}
