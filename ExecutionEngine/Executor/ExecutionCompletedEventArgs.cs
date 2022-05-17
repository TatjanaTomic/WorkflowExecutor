using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Executor
{
    public class ExecutionCompletedEventArgs
    {
        private Xml.Step step;
        private bool isSuccessful;

        public ExecutionCompletedEventArgs(Xml.Step step, bool isSuccessful)
        {
            this.step = step;
            this.isSuccessful = isSuccessful;
        }

        public Xml.Step Step { get => step; set => this.step = value; }
        public bool IsSuccessful { get => isSuccessful; set => this.isSuccessful = value; }
    }
}
