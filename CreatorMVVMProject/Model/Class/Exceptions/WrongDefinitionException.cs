using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
