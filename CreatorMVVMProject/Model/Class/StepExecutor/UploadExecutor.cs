using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Class.StepExecutor
{
    public class UploadExecutor : AbstractExecutor
    {
        private static readonly string BASE_PATH = Path.Combine(Environment.GetFolderPath(folder: Environment.SpecialFolder.Desktop), "test");
        private readonly Step step;
        public UploadExecutor(Step step)
        {
            this.step = step;
        }

        public async override Task Start()
        {
            OnExecutionStarted(step);

            await Task.Run( () =>
            {
                if(!File.Exists(step.File))
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, "error"));
                    return;
                }

                
                //TODO : Dovrsi ovo


            });
        }

        public override Task Stop()
        {
            throw new NotImplementedException();
        }
    }
}
