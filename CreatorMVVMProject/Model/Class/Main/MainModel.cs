using CreatorMVVMProject.Model.Interface.WorkflowService;
using System.Collections.Generic;
using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using CreatorMVVMProject.Model.Interface.ExecutionService;
using System;

namespace CreatorMVVMProject.Model.Class.Main
{

    // TODO: Promijeni apsolutne putanje u workflow fajlu (Dodaj upload path u App.config)
    // TODO: Precisti kod, obrisi nekoristene using-e i slicno
    // TODO: Provjeri da li ima binding errora
    // TODO: Napisi dokumentaciju za komplikovanije metode
    // TODO: Sredi izgled Select steps Dialog Box-a

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


        public IList<StageStatus> Stages
        {
            get => this.statusReportService.Stages;
        }

        public void AddStepsToExecution(List<StepStatus> steps)
        {
            executionService.ExecuteSelectedSteps(steps);
        }

        public IExecutionService ExecutionService
        {
            get => this.executionService;
        }

        public event EventHandler? ExecutionCompleted;
        private void StepsExecutionCompleted(object? _, EventArgs e)
        {
            ExecutionCompleted?.Invoke(this, e);
        }

        public event EventHandler? ExecutionSelectedStepsStarted;
        private void SelectedStepsExecutionStarted(object? _, EventArgs e)
        {
            ExecutionSelectedStepsStarted?.Invoke(this, e);
        }

        public event EventHandler? ExecutionTillThisStepStarted;
        private void ExecutionTillThisStarted(object? _, EventArgs e)
        {
            ExecutionTillThisStepStarted?.Invoke(this, e);
        }
    }
}
