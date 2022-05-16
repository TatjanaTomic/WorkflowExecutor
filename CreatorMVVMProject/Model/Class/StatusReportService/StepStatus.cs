using CreatorMVVMProject.Model.Interface.StatusReportService;
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
        //ne treba ni executor u sebi imati
        private readonly AbstractExecutor? executor;
        //ovaj dio izbaci
        //step status ne treba da zna o dependency stepovima
        //to sve radi servis
        //servis setuje inicijalni status, mijenja status tokom rada i mijenja statuse zavisnim stepovima po potrebi
        //private readonly IList<Step> firstLevelDependencySteps;
        //private readonly IList<Step> allDependencySteps;

        private readonly IStatusReportService statusReportService;

        public StepStatus(Step step, Status initialStatus, IStatusReportService statusReportService)
        {
            this.step = step;
            this.status = initialStatus;
            this.statusReportService = statusReportService;

            this.executor = ExecutorFabrique.Instance.CreateExecutor(step);
            if (executor != null)
            {
                executor.ExecutionStarted += StepExecutionStarted;
                executor.ExecutionCompleted += StepExecutionCompleted;
            }
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

        public void StepExecutionStarted()
        {
            statusReportService.SetStatusToStep(this, Status.InProgress); 
        }

        // zasad su svi uspjesni
        public void StepExecutionCompleted()
        {
            statusReportService.SetStatusToStep(this, Status.Success);
        }
    }
}
