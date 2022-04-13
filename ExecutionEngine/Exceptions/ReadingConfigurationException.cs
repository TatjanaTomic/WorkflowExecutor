using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Exceptions
{
    public class ReadingConfigurationException : Exception
    {
        public ReadingConfigurationException()
        {
        }

        public ReadingConfigurationException(string message)
            : base(message)
        {
        }

        public ReadingConfigurationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}