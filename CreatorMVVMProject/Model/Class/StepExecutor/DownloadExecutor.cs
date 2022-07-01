using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;

namespace CreatorMVVMProject.Model.Class.StepExecutor
{
    public class DownloadExecutor : AbstractExecutor
    {
        private static readonly string? downloadsPath = ConfigurationManager.AppSettings["downloadsPath"]?.ToString();
        private static readonly HttpClient httpClient = new();
        private readonly Step step;

        public DownloadExecutor(Step step)
        {
            this.step = step;
        }

        /// <summary>
        /// Method starts executino of Step which type is Download. It checks if the specified File property of Step is formatted correctly.
        /// Method checks if downloads path is specified in Application configuration file.
        /// It downloads the file from a Server to a specified path.
        /// Method raises events when execution starts and when it is completed.
        /// </summary>
        public override async Task Start()
        {
            OnExecutionStarted(step);

            await Task.Run(async () =>
            {
                if (!Uri.TryCreate(step.File, UriKind.Absolute, out Uri? uriResult))
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, "Step property File is not valid."));
                    return;
                }

                if (downloadsPath == null)
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, "Downloads path is not defined."));
                    return;
                }

                var resultPath = Path.Combine(downloadsPath, Path.GetFileName(step.File));

                try
                {
                    var fileBytes = await httpClient.GetByteArrayAsync(uriResult);

                    File.WriteAllBytes(resultPath, fileBytes);

                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, true, "File " + Path.GetFileName(resultPath) + " downloaded successuflly."));
                }
                catch (Exception ex)
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, ex.Message));
                }

            });

        }
    }
}
