using ExecutionEngine.Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Executor
{
    public class ExecutorFabrique
    {
        private static ExecutorFabrique? instance;

        private ExecutorFabrique()
        {
            
        }

        public static ExecutorFabrique Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ExecutorFabrique();
                }
                return instance;
            }
        }

        public AbstractExecutor? CreateExecutor(Xml.Step step)
        {
            AbstractExecutor? executor = null;
            switch (step.Type)
            {
                case Step.Type.Executable:
                    executor = new ScriptExecutor(step);
                    break;

                case Step.Type.Upload:
                    executor = new UploadExecutor();
                    break;

                case Step.Type.Download:
                    executor = new DownloadExecutor();
                    break;
            }
            return executor;
        }
    }
}
