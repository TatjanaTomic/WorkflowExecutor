using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Executor
{
    
    public abstract class AbstractExecutor
    {
        //dodati event execution finished koji prosljedjuje status
        //kroz runove envoke/ati event

        //u step statusu staviti event handler za taj event
        public abstract Task Start();
        public abstract Task Stop();
    }
}
