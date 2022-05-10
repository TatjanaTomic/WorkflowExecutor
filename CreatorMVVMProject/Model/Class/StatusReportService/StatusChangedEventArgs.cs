using ExecutionEngine.Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Class.StatusReportService
{
    public class StatusChangedEventArgs
    {
        private Status status;
        private string stepId = string.Empty;
        public StatusChangedEventArgs(Status status, string stepId)
        {
            this.status = status;
            this.stepId = stepId;
        }
        public Status Status { get => status; set => this.status = value; }
        public string StepId { get => stepId; set => this.stepId = value; }
    }
}
