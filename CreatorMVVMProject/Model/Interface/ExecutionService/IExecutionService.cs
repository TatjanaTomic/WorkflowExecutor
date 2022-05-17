using CreatorMVVMProject.Model.Class.StatusReportService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Interface.ExecutionService
{
    public interface IExecutionService
    {
        //Task EnqueueSteps(List<StepStatus> stepsToExecute);
        void EnqueueSteps(List<StepStatus> stepsToExecute);
    }
}
