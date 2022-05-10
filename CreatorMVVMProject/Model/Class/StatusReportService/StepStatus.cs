﻿using CreatorMVVMProject.Model.Interface.StatusReportService;
using ExecutionEngine.Executor;
using ExecutionEngine.Step;
using ExecutionEngine.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Class.StatusReportService
{
    public class StepStatus
    {
        private readonly Step step;
        private Status status;
        private readonly AbstractExecutor? executor;

        private readonly IStatusReportService statusReportService;

        public StepStatus(Step step, IStatusReportService statusReportService)
        {
            this.step = step;
            
            executor = ExecutorFabrique.Instance.CreateExecutor(step);
            if(executor != null)
                executor.ExecutionStarted += StepExecutionStarted;

            SetInitialStatus();

            this.statusReportService = statusReportService;
        }

        public Step Step
        {
            get { return this.step; }
        }

        public AbstractExecutor? Executor 
        {
            get { return this.executor; }
        }

        public event EventHandler<StatusChangedEventArgs>? StatusChanged;
        public Status Status
        {
            get => this.status;
            set
            {
                this.status = value;
                StatusChanged?.Invoke(this, new StatusChangedEventArgs(status, step.Id));
            }
        }
        
        private void SetInitialStatus()
        {
            if(step != null && step.Dependencies != null)
            {
                if (step.Dependencies.Count > 0)
                    status = Status.Disabled;
                else
                    status = Status.NotStarted;
            }
        }


        public void StepExecutionStarted()
        {
            statusReportService.SetStatusToStep(this, Status.InProgress);
        }
    }
}