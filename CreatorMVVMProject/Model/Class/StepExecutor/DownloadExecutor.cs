using System;
using System.Threading.Tasks;
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
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, "Step property File is not valid."));
                    return;
                }

                if(downloadsPath == null)
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, "Downloads path is not defined."));
                    return;
                }

                string resultPath = Path.Combine(downloadsPath, Path.GetFileName(step.File));

                try
                {
                    byte[] fileBytes = await httpClient.GetByteArrayAsync(uriResult);
                    
                    File.WriteAllBytes(resultPath, fileBytes);

                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, true, "File " + Path.GetFileName(resultPath) +  " downloaded successuflly."));
                }
                catch (Exception ex)
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, ex.Message));
                }
                
            });

        }
    }
}
