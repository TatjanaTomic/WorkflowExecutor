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
        private Xml.Step step;
        public ScriptExecutor(Xml.Step step)
        {
            this.step = step;
        }

        public override async Task Start()
        {
            OnExecutionStarted();

            await Task.Run(() => {

                string command = "/C " + step.ExecutablePath + " " + BuildParameters(step.Parameters);
                Console.WriteLine("      Command: " + command);

                ProcessStartInfo startInfo = new("cmd.exe", command);
                using Process? process = Process.Start(startInfo);
                
            });
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
    }
}
