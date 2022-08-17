using System;
using System.Collections.Generic;
using CreatorMVVMProject.Model.Class.ExecutionService;
using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Interface.DialogService;
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
        private readonly IDialogService dialogService;

        public MainModel(IStatusReportService statusReportService, IWorkflowService workflowService, IExecutionService executionService, IDialogService dialogService)
        {
            this.statusReportService = statusReportService;
            this.workflowService = workflowService;
            this.executionService = executionService;
            this.executionService.ExecutionCompleted += StepsExecutionCompleted;
            this.executionService.ExecutionSelectedStepsStarted += SelectedStepsExecutionStarted;
            this.executionService.ExecutionTillThisStepStarted += ExecutionTillThisStarted;
            this.dialogService = dialogService;
        }

        public event EventHandler<ExecutionEventArgs>? ExecutionCompleted;
        public event EventHandler<ExecutionEventArgs>? ExecutionTillThisStepStarted;
        public event EventHandler<ExecutionEventArgs>? ExecutionSelectedStepsStarted;

        public IList<StageStatus> Stages => statusReportService.Stages;

        public IExecutionService ExecutionService => executionService;

        public IDialogService DialogService => dialogService;


        private void StepsExecutionCompleted(object? _, ExecutionEventArgs args)
        {
            ExecutionCompleted?.Invoke(this, args);
        }

        private void ExecutionTillThisStarted(object? _, ExecutionEventArgs args)
        {
            ExecutionTillThisStepStarted?.Invoke(this, args);
        }

        private void SelectedStepsExecutionStarted(object? _, ExecutionEventArgs args)
        {
            ExecutionSelectedStepsStarted?.Invoke(this, args);
        }

        public void AddStepsToExecution(List<StepStatus> steps)
        {
            executionService.ExecuteSelectedSteps(steps);
        }
    }
}
