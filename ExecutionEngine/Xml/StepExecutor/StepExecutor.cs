using ExecutionEngine.Step;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Xml.StepExecutor
{
    internal class StepExecutor
    {
        public static void ExecuteStep(Step step)
        {
            //TODO : Provjeriti prvo da li su zavisni koraci izveseni

            switch (step.Type)
            {
                case StepType.Executable:
                    if (!string.IsNullOrEmpty(step.ExecutablePath))
                        ExecuteScript(step.ExecutablePath, step.Parameters);
                    break;
            }
        }

        private static void ExecuteScript(string executablePath, List<Parameter>? parameters)
        {
            //TODO : NEDOVRSENO IZVRSAVANJE SKRIPTE

            //Pokretanje shall skripte
            string command = "/C " + executablePath + " " + BuildParameters(parameters);
            Console.WriteLine("      Command: " + command);

            ProcessStartInfo startInfo = new("cmd.exe", command);
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            //startInfo.FileName = executablePath;
            //startInfo.Arguments = BuildParameters(parameters);

            using Process? process = Process.Start(startInfo);        
            if (process != null)
            {
                using StreamReader reader = process.StandardOutput;
                string result = reader.ReadToEnd();
                Console.Write(result);
            }

            //process.WaitForExit();
            //process.WaitForExitAsync();
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
