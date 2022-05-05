﻿using ExecutionEngine.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Executor
{
    public class ScriptExecutor : AbastractExecutor
    {
        private Xml.Step step;
        public ScriptExecutor(Xml.Step step)
        {
            this.step = step;
        }

        public override async Task Start()
        {
            await Task.Run(() => {
                // TODO : Zavrsi / provjeri ovo

                string command = "/C " + step.ExecutablePath + " " + BuildParameters(step.Parameters);
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
            });
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
    }
}