using ExecutionEngine.Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Stage
{
    public interface IStage<IStep> 
    {
        string Id { get; }
        List<IStep>? Steps { get; }
    }
}
