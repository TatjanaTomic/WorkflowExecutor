using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;

namespace CreatorMVVMProject.Model.Class.Executor
{
    public class DownloadExecutor : AbstractExecutor
    {
        private static readonly string BASE_PATH = Path.Combine(Environment.GetFolderPath(folder: Environment.SpecialFolder.Desktop), "test");

        private readonly Step step;
        public DownloadExecutor(Step step)
        {
            this.step = step;
        }

        public async override Task Start()
        {
            bool isSuccessful = true;
            OnExecutionStarted(step);

            await Task.Run(() =>
            {
                try
                {
                    //TODO : Nadji neki elegantniji nacin, WebClient je depricated !
                    // HttpClient
                    using (WebClient client = new())
                    {
                        client.DownloadFile(step.File, Path.Combine(BASE_PATH, "testic.txt"));
                    }
                }
                catch (Exception ex)
                {
                    //TODO : Sta je "pravilno" ponasanje kada se desi Exception? Da li da negdje logujem Exception-e?
                    isSuccessful = false;
                }
            });

            OnExecutionCompleted(new ExecutionCompletedEventArgs(step, isSuccessful));

        }
        public override Task Stop()
        {
            throw new NotImplementedException();
        }
    }
}
