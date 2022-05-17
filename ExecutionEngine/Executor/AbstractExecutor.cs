﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Executor
{
    //public delegate void Notify(Xml.Step step);
    

    public abstract class AbstractExecutor
    {
        public event EventHandler<Xml.Step>? ExecutionStarted;
        public event EventHandler<ExecutionCompletedEventArgs>? ExecutionCompleted;

        protected virtual void OnExecutionStarted(Xml.Step e)
        {
            ExecutionStarted?.Invoke(this, e);
        }

        protected virtual void OnExecutionCompleted(ExecutionCompletedEventArgs e)
        {
            ExecutionCompleted?.Invoke(this, e);
        }

        public abstract Task Start();
        public abstract Task Stop();
    }
}
