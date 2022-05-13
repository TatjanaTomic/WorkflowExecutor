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
        private readonly AbstractExecutor? executor;
        //ovaj dio izbaci
        //step status ne treba da zna o dependency stepovima
        //to sve radi servis
        //servis setuje inicijalni status, mijenja status tokom rada i mijenja statuse zavisnim stepovima po potrebi
        private readonly IList<Step> firstLevelDependencySteps;
        private readonly IList<Step> allDependencySteps;

        private readonly IStatusReportService statusReportService;

        public StepStatus(Step step, IStatusReportService statusReportService)
        {
            this.step = step;
            this.statusReportService = statusReportService;

            this.executor = ExecutorFabrique.Instance.CreateExecutor(step);
            if(executor != null)
                executor.ExecutionStarted += StepExecutionStarted;

            this.firstLevelDependencySteps = statusReportService.GetFirstLevelDependencySteps(Step);
            this.allDependencySteps = statusReportService.GetAllDependencySteps(Step);

            SetInitialStatus();
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
        //ide u status report servis
        private void SetInitialStatus()
        {
            if(firstLevelDependencySteps.Count > 0)
                status = Status.Disabled;
            else
                status = Status.NotStarted;
        }

        public void StepExecutionStarted()
        {
            statusReportService.SetStatusToStep(this, Status.InProgress);
        }
    }
}
