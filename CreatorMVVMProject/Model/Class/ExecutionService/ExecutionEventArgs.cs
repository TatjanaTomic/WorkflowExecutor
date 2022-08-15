using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Class.ExecutionService
{
    public class ExecutionEventArgs
    {
        public ExecutionEventArgs(string message, bool executionFailed)
        {
            Message = message;
            ExecutionFailed = executionFailed;
        }

        public string Message { get; set; }

        public bool ExecutionFailed { get; set; }
    }
}
