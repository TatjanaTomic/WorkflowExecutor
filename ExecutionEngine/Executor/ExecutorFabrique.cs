using ExecutionEngine.Exceptions;
using ExecutionEngine.Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Executor
{
    //TODO : Da li ovdje zaista trebam Singleton ?
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
                return instance ??= new ExecutorFabrique();
            }
        }

        public AbstractExecutor CreateExecutor(Xml.Step step)
        {
            return step.Type switch
            {
                Step.Type.Executable => new ScriptExecutor(step),
                Step.Type.Upload => new UploadExecutor(),
                Step.Type.Download => new DownloadExecutor(),
                _ => throw new WrongDefinitionException("Step type must be defined.")
            };
        }
    }
}
