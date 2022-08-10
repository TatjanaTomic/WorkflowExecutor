using System;
using System.Collections.Generic;
using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Interface.ExecutionService;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using CreatorMVVMProject.Model.Interface.WorkflowService;

namespace CreatorMVVMProject.Model.Class.Main
{
    public class MainModel
    {
        private readonly IStatusReportService statusReportService;
        private readonly IWorkflowService workflowService;
        private readonly IExecutionService executionService;

        public MainModel(IStatusReportService statusReportService, IWorkflowService workflowService, IExecutionService executionService)
        {
            this.statusReportService = statusReportService;
            this.workflowService = workflowService;
            this.executionService = executionService;
            this.executionService.ExecutionCompleted += StepsExecutionCompleted;
            this.executionService.ExecutionSelectedStepsStarted += SelectedStepsExecutionStarted;
            this.executionService.ExecutionTillThisStepStarted += ExecutionTillThisStarted;
        }
        
        public event EventHandler<string>? ExecutionCompleted;
        public event EventHandler<string>? ExecutionTillThisStepStarted;
        public event EventHandler<string>? ExecutionSelectedStepsStarted;
        
        public IList<StageStatus> Stages => statusReportService.Stages;

        public IExecutionService ExecutionService => executionService;

        
        private void StepsExecutionCompleted(object? _, string message)
        {
            ExecutionCompleted?.Invoke(this, message);
        }
        
        private void ExecutionTillThisStarted(object? _, string message)
        {
            ExecutionTillThisStepStarted?.Invoke(this, message);
        }

        private void SelectedStepsExecutionStarted(object? _, string message)
        {
            ExecutionSelectedStepsStarted?.Invoke(this, message);
        }

        public void AddStepsToExecution(List<StepStatus> steps)
        {
            executionService.ExecuteSelectedSteps(steps);
        }
    }
}
