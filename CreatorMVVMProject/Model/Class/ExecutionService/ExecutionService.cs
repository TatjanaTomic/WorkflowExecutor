using CreatorMVVMProject.Model.Interface.ExecutionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using ExecutionEngine.Xml;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using CreatorMVVMProject.Model.Class.StatusReportService;

namespace CreatorMVVMProject.Model.Class.ExecutionService
{
    public class ExecutionService : IExecutionService
    {
        //ovdje ide logika za queue stepova koji idu na izvrsavanje
        //zamisljeno je kao lista lista
        //stepovi koji se izvrsavaju paralelno idu u jednu listu
        //za svaki step koji se izvrsava sekvencijalno pravi se lista koja ima jedan element
        //odavde se koristi step executor
        //queue prima step statuse

        private readonly IStatusReportService statusReportService;

        private readonly ConcurrentQueue<List<StepStatus>> stepsQueue = new();

        public ExecutionService(IStatusReportService statusReportService)
        {
            this.statusReportService = statusReportService;
        }

        public void EnqueueSteps(List<StepStatus> stepsToExecute)
        {
            List<StepStatus> parallelSteps = new();

            foreach (StepStatus stepStatus in stepsToExecute)
            {
                if (stepStatus.Step.CanBeExecutedInParallel)
                    parallelSteps.Add(stepStatus);
                else
                    stepsQueue.Enqueue(new List<StepStatus> { stepStatus });
            }
            stepsQueue.Enqueue(parallelSteps);
        }

        public void StartExecution()
        {

        }
    }
}
