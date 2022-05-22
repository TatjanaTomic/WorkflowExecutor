using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Class.StepExecutor
{
    public abstract class AbstractExecutor
    {
        public event EventHandler<Step>? ExecutionStarted;
        public event EventHandler<ExecutionCompletedEventArgs>? ExecutionCompleted;

        public abstract Task Start();
        public abstract Task Stop();

        protected virtual void OnExecutionStarted(Step e)
        {
            ExecutionStarted?.Invoke(this, e);
        }

        protected virtual void OnExecutionCompleted(ExecutionCompletedEventArgs e)
        {
            ExecutionCompleted?.Invoke(this, e);
        }
    }
}
