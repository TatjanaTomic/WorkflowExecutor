﻿using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
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
        readonly ProcessStartInfo processStartInfo = new();

        private readonly Step step;
        public ScriptExecutor(Step step)
        {
            this.step = step;

            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.CreateNoWindow = true;
            //processStartInfo.UseShellExecute

            processStartInfo.WorkingDirectory = BASE_PATH;
            processStartInfo.FileName = "cmd.exe";
            processStartInfo.Arguments = "/C " + step.ExecutablePath + " " + BuildParameters();
        }

        public async override Task Start()
        {
            OnExecutionStarted(step);
            await Task.Run(() =>
            {
                try
                {
                    Process? process = Process.Start(processStartInfo);
                    if (process != null)
                    {
                        process.EnableRaisingEvents = true;
                        process.Exited += new EventHandler(ProcessExited);
                    }
                    else
                        //TODO : Promijeni poruku
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
    }
}
