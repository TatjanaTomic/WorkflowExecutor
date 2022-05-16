using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Executor
{
    public delegate void Notify();

    public abstract class AbstractExecutor
    {
        public event Notify? ExecutionStarted;
        public event Notify? ExecutionCompleted;

        protected virtual void OnExecutionStarted()
        {
            ExecutionStarted?.Invoke();
        }

        protected virtual void OnExecutionCompleted()
        {
            ExecutionCompleted?.Invoke();
        }

        public abstract Task Start();
        public abstract Task Stop();
    }
}
