using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Class.StepExecutor
{
    public class UploadExecutor : AbstractExecutor
    {
        private static readonly string? httpClientBaseAddress = ConfigurationManager.AppSettings["httpClientBaseAddress"]?.ToString();
        private static readonly string? uploadPath = ConfigurationManager.AppSettings["uploadPath"]?.ToString();
        private static readonly HttpClient httpClient = new()
        {
            BaseAddress = new(httpClientBaseAddress)
        };

        private readonly Step step;

        public UploadExecutor(Step step)
        {
            this.step = step;
        }

        public override async Task Start()
        {
            OnExecutionStarted(step);

            if(uploadPath == null)
            {
                OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, "Uploads path is not defined."));
                return;
            }

            var fileName = Path.GetFileName(step.File);
            var filePath = Path.Combine(uploadPath, fileName);

            await Task.Run(async () =>
            {
                if (!File.Exists(filePath))
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, "File " + fileName + " does not exist."));
                    return;
                }

                try
                {
                    await using FileStream stream = File.OpenRead(filePath);
                    using HttpRequestMessage request = new(HttpMethod.Post, "file");
                    using MultipartFormDataContent content = new()
                    {
                        { new StreamContent(stream), "file", fileName }
                    };

                    request.Content = content;

                    await httpClient.SendAsync(request);

                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, true, "File " + fileName +  " downloaded successuflly."));
                }
                catch (Exception ex)
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, ex.Message));
                }
            });
        }
        
    }
}
