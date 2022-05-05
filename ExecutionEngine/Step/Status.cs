using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Step
{
    //dodan disabled status - step se ne moze izvrsiti jer se njegovi zavrsni nisu izvrsili
    public enum Status
    {
        NotStarted,
        InProgress,
        Success,
        Filed,
        Obsolete,
        Disabled
    }
}
