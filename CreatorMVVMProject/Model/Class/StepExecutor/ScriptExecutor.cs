using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Class.StepExecutor
{
    public class ScriptExecutor : AbstractExecutor
    {
        private static readonly string BASE_PATH = Path.Combine(Environment.GetFolderPath(folder: Environment.SpecialFolder.Desktop), "test");

        private readonly Step step;
        public ScriptExecutor(Step step)
        {
            this.step = step;
        }

        public async override Task Start()
        {
            OnExecutionStarted(step);
            await Task.Run(() =>
            {
                ProcessStartInfo processStartInfo = new();
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.UseShellExecute = false;

                processStartInfo.WorkingDirectory = BASE_PATH;
                processStartInfo.FileName = "cmd.exe";
                processStartInfo.Arguments = "/C " + step.ExecutablePath + " " + BuildParameters();
                
                try
                {
                    Process? process = Process.Start(processStartInfo);
                    if (process != null)
                    {
                        process.WaitForExit();
                        
                        using StreamReader outputReader = process.StandardOutput;
                        using StreamReader errorReader = process.StandardError;

                        string output = outputReader.ReadToEnd();
                        string error = errorReader.ReadToEnd();

                        if (!string.IsNullOrEmpty(output))
                        {
                            OnExecutionCompleted(new ExecutionCompletedEventArgs(step, true, output));
                        }
                        else if(!string.IsNullOrEmpty(error))
                        {
                            OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, error));
                        }
                    }
                    else
                        OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, "Moja neka poruka"));
                }
                catch (Exception e)
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, e.Message));
                }
            });
        }

        public override Task Stop()
        {
            throw new NotImplementedException();
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
    }
}
