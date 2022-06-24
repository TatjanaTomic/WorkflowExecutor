using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Class.StepExecutor
{
    public class ScriptExecutor : AbstractExecutor
    {
        private readonly Step step;
        readonly ProcessStartInfo processStartInfo = new();

        private readonly string? basePath = ConfigurationManager.AppSettings["basePath"]?.ToString();

        public ScriptExecutor(Step step)
        {
            this.step = step;

            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.CreateNoWindow = true;

            processStartInfo.WorkingDirectory = basePath;
            processStartInfo.FileName = ConfigurationManager.AppSettings["processStartInfoFileName"]?.ToString();

            string command = ConfigurationManager.AppSettings["processStartInfoCommand"]?.ToString() + " " + step.ExecutablePath + " " + BuildParameters();
            processStartInfo.Arguments = command;
        }

        public async override Task Start()
        {
            OnExecutionStarted(step);
            await Task.Run(() =>
            {
                try
                {
                    Process? process = Process.Start(processStartInfo);

                    if (process == null)
                    {
                        OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, "Moja neka poruka"));
                        return;
                    }

                    process.EnableRaisingEvents = true;
                    process.Exited += new EventHandler(ProcessExited);

                    process.WaitForExit();
                }
                catch (Exception e)
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, e.Message));
                }
            });
        }

        private string BuildParameters()
        {
            StringBuilder stringBuilder = new();
            foreach (var parameter in step.Parameters)
            {
                stringBuilder.Append(parameter.ToString());
            }

            return stringBuilder.ToString();
        }

        private void ProcessExited(object? sender, EventArgs e)
        {
            if(sender is not Process)
            {
                return;
            }
            
            Process process = sender as Process;

            if (process.ExitCode != 0)
            {
                string error = process.StandardError.ReadToEnd();
                OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, error));
                return;
            }

            string output = process.StandardOutput.ReadToEnd();
            OnExecutionCompleted(new ExecutionCompletedEventArgs(step, true, output));    
        }

        public override Task Stop()
        {
            throw new NotImplementedException();
        }
    }
}
