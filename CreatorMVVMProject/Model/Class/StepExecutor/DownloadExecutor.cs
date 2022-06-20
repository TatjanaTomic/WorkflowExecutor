using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using System.Net.Http;
using System.Configuration;

namespace CreatorMVVMProject.Model.Class.StepExecutor
{
    public class DownloadExecutor : AbstractExecutor
    {
        private readonly string? downloadsPath = ConfigurationManager.AppSettings["downloadsPath"]?.ToString();

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
                if (!Uri.TryCreate(step.File, UriKind.Absolute, out Uri? uriResult))
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, "Step property File is invalid."));
                    return;
                }

                string path = Path.Combine(downloadsPath, Path.GetFileName(step.File));

                try
                {
                    byte[] fileBytes = await httpClient.GetByteArrayAsync(uriResult);
                    
                    File.WriteAllBytes(path, fileBytes);

                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, true, "Downloaded"));
                }
                catch (Exception ex)
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, ex.Message));

                }
                
            });

        }

        public override Task Stop()
        {
            throw new NotImplementedException();
        }
    }
}
