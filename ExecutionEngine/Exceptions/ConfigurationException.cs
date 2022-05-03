using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Exceptions
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException()
        {
        }

        public ConfigurationException(string message)
            : base(message)
        {
        }

        public ConfigurationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}