using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using System.Net.Http;

namespace CreatorMVVMProject.Model.Class.StepExecutor
{
    public class DownloadExecutor : AbstractExecutor
    {
        private static readonly string BASE_PATH = Path.Combine(Environment.GetFolderPath(folder: Environment.SpecialFolder.Desktop), "test");
        readonly HttpClient httpClient = new();
        
        private readonly Step step;
        public DownloadExecutor(Step step)
        {
            this.step = step;
        }

        public async override Task Start()
        {
            OnExecutionStarted(step);

            await Task.Run(async () =>
            {
                if (!Uri.TryCreate(step.File, UriKind.Absolute, out Uri uriResult))
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, "URI is invalid."));
                    return;
                }
                    
                byte[] fileBytes = await httpClient.GetByteArrayAsync(step.File);

                //TODO : Gdje cuvam preuzete fajlove ?
                string testPath = Path.Combine(BASE_PATH, "testXXX.txt");
                File.WriteAllBytes(testPath, fileBytes);

                OnExecutionCompleted(new ExecutionCompletedEventArgs(step, true, "Downloaded"));
            });

        }

        public override Task Stop()
        {
            throw new NotImplementedException();
        }
    }
}
