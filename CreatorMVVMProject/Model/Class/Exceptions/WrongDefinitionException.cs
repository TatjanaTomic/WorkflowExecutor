using System;
using System.Runtime.Serialization;

namespace CreatorMVVMProject.Model.Class.Exceptions
{
    [Serializable]
    public class WrongDefinitionException : Exception
    {
        public WrongDefinitionException()
        {
        }

        public WrongDefinitionException(string message)
            : base(message)
        {
        }

        public WrongDefinitionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected WrongDefinitionException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}
