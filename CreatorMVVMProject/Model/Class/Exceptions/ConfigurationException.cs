using System;
using System.Runtime.Serialization;

namespace CreatorMVVMProject.Model.Class.Exceptions
{
    [Serializable]
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

        protected ConfigurationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}
