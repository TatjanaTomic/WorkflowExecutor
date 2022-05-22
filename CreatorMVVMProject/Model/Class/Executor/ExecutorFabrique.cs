using CreatorMVVMProject.Model.Class.Exceptions;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml.Type;

namespace CreatorMVVMProject.Model.Class.Executor
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
                return instance ??= new ExecutorFabrique();
            }
        }

        public AbstractExecutor CreateExecutor(Step step)
        {
            return step.Type switch
            {
                Type.Executable => new ScriptExecutor(step),
                Type.Upload => new UploadExecutor(),
                Type.Download => new DownloadExecutor(step),
                _ => throw new WrongDefinitionException("Step type must be defined.")
            };
        }
    }
}
