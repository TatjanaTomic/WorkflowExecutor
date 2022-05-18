using ExecutionEngine.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Executor
{
    public class ScriptExecutor : AbstractExecutor
    {
        private readonly Xml.Step step;
        public ScriptExecutor(Xml.Step step)
        {
            this.step = step;
        }

        public async override Task Start()
        {
            OnExecutionStarted(step);

            Random random = new();
            int time = random.Next(10) * 1000;

            await Task.Run(() =>
            {

                string command = "/C " + step.ExecutablePath + " " + BuildParameters(step.Parameters);

                ProcessStartInfo startInfo = new("cmd.exe", command);
                using Process? process = Process.Start(startInfo);

                //TODO : obrisi sleep
                Thread.Sleep(3000);
            });

            OnExecutionCompleted(new ExecutionCompletedEventArgs(step, true));
        }

        public override Task Stop()
        {
            throw new NotImplementedException();
        }

        private static string BuildParameters(List<Parameter>? parameters)
        {
            StringBuilder sb = new();
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    sb.Append(p.KeyWord);
                    sb.Append(' ');
                    sb.Append(p.Value);
                }
            }

            return sb.ToString();
        }

        //startInfo.UseShellExecute = false;
        //startInfo.RedirectStandardOutput = true;
        //startInfo.FileName = executablePath;
        //startInfo.Arguments = BuildParameters(parameters);

        /* kako da se procita output ako je RedirectStandardOutput postavljen na true
            if (process != null)
            {
                using StreamReader reader = process.StandardOutput;
                string result = reader.ReadToEnd();
            }
        */

        //process.WaitForExit();
        //process.WaitForExitAsync();
    }
}
