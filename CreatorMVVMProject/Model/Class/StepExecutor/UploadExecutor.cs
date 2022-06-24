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
        private readonly Step step;

        private static readonly string httpClientBaseAddress = ConfigurationManager.AppSettings["httpClientBaseAddress"]?.ToString();
        readonly HttpClient httpClient = new()
        {
            BaseAddress = new(httpClientBaseAddress)
        };

        public UploadExecutor(Step step)
        {
            this.step = step;
        }

        public async override Task Start()
        {
            OnExecutionStarted(step);

            await Task.Run(async () =>
            {
                if (!File.Exists(step.File))
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, "File " + step.File + " does not exist."));
                    return;
                }

                string fileName = Path.GetFileName(step.File);

                try
                {
                    await using FileStream stream = File.OpenRead(step.File);
                    using HttpRequestMessage request = new(HttpMethod.Post, "file");
                    using MultipartFormDataContent content = new()
                    {
                        { new StreamContent(stream), "file", fileName }
                    };

                    request.Content = content;

                    await httpClient.SendAsync(request);

                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, true, "OK"));
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
