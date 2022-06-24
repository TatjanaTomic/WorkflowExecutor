using System;

namespace CreatorMVVMProject.Model.Class.Exceptions
{
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
    }
}
