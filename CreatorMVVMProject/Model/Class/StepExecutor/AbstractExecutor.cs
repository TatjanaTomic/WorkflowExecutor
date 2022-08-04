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

        protected void OnExecutionStarted(Step e)
        {
            ExecutionStarted?.Invoke(this, e);
        }

        protected void OnExecutionCompleted(ExecutionCompletedEventArgs e)
        {
            ExecutionCompleted?.Invoke(this, e);
        }
    }
}
