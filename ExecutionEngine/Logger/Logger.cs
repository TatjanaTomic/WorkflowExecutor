using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Logger
{
    public static class Logger
    {
        private static readonly string ROOT_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly string LOGS_PATH = Path.Combine(ROOT_PATH, "WorkflowExecutorTest");

        public static void Logs(string message, string fileName)
        {
            if (!Directory.Exists(LOGS_PATH))
            {
                Directory.CreateDirectory(LOGS_PATH);
            }

            var path = Path.Combine(LOGS_PATH, fileName);
            using TextWriter writer = new StreamWriter(new FileStream(path, FileMode.Append));
            writer.WriteLine(message);
        }
    }
}
