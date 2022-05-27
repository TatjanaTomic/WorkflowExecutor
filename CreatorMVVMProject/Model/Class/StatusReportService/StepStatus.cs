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

        public string StatusMessage
        {
            get { return statusMessage; }
            set { statusMessage = value;
                MessageChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public Status Status
        {
            get => this.status;
            set
            {
                this.status = value;
                StatusChanged?.Invoke(this, new StatusChangedEventArgs(status, step.Id));
            }
        }

        public event EventHandler? MessageChanged;
        public event EventHandler<StatusChangedEventArgs>? StatusChanged;
    }
}
