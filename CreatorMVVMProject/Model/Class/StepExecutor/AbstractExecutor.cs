using System;
using System.Threading.Tasks;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;

namespace CreatorMVVMProject.Model.Class.StepExecutor
{
    public abstract class AbstractExecutor
    {
        public event EventHandler<Step>? ExecutionStarted;
        public event EventHandler<ExecutionCompletedEventArgs>? ExecutionCompleted;

        public abstract Task Start();

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
