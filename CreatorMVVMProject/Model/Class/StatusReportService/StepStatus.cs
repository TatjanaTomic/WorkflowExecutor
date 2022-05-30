using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Interface.StatusReportService;
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
        private string statusMessage = string.Empty;
        private bool canBeExecuted;

        private readonly IStatusReportService statusReportService;

        public StepStatus(Step step, Status initialStatus, bool canBeExecuted, IStatusReportService statusReportService)
        {
            this.step = step;
            this.status = initialStatus;
            this.canBeExecuted = canBeExecuted;
            this.statusReportService = statusReportService;
        }

        public Step Step
        {
            get { return this.step; }
        }

        public string StatusMessage
        {
            get { return statusMessage; }
            set
            {
                statusMessage = value;
                MessageChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Status Status
        {
            get { return status; }
            set
            {
                this.status = value;
                StatusChanged?.Invoke(this, new StatusChangedEventArgs(status, step.Id));
            }
        }

        public bool CanBeExecuted
        {
            get { return canBeExecuted; }
            set
            {
                canBeExecuted = value;
                CanBeExecutedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? MessageChanged;
        public event EventHandler? CanBeExecutedChanged;
        public event EventHandler<StatusChangedEventArgs>? StatusChanged;

    }
}
