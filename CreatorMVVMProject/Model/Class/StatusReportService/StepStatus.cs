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

        private readonly IStatusReportService statusReportService;

        public StepStatus(Step step, Status initialStatus, IStatusReportService statusReportService)
        {
            this.step = step;
            this.status = initialStatus;
            this.statusReportService = statusReportService;
        }

        public Step Step
        {
            get { return this.step; }
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
    }
}
