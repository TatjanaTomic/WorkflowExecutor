using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;

namespace CreatorMVVMProject.Model.Class.StepExecutor;

public class ScriptExecutor : AbstractExecutor
{
    private static readonly string? executablesPath = ConfigurationManager.AppSettings["executablesPath"]?.ToString();
    private readonly Step step;
    private readonly ProcessStartInfo processStartInfo = new();

    public ScriptExecutor(Step step)
    {
        this.step = step;

        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.RedirectStandardError = true;
        processStartInfo.CreateNoWindow = true;

        processStartInfo.WorkingDirectory = executablesPath;
        processStartInfo.FileName = ConfigurationManager.AppSettings["processStartInfoFileName"]?.ToString();

        var command = ConfigurationManager.AppSettings["processStartInfoCommand"]?.ToString() + " " + step.ExecutablePath + " " + BuildParameters();
        processStartInfo.Arguments = command;
    }

    /// <summary>
    /// Method starts execution of Step which type is Executable. It starts new local system process based on ProcessStartInfo object.
    /// Method raises events when execution starts and when it is completed.
    /// </summary>
    public override async Task Start()
    {
        OnExecutionStarted(step);
        await Task.Run(() =>
        {
            try
            {
                Process? process = Process.Start(processStartInfo);

                if (process == null)
                {
                    OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, "Unable to start execution of step."));
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
        if (sender is not Process process)
        {
            return;
        }

        if (process.ExitCode != 0)
        {
            var error = process.StandardError.ReadToEnd();
            OnExecutionCompleted(new ExecutionCompletedEventArgs(step, false, error));
            return;
        }

        var output = process.StandardOutput.ReadToEnd();
        OnExecutionCompleted(new ExecutionCompletedEventArgs(step, true, output));
    }

}
