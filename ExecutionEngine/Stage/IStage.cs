using ExecutionEngine.Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Stage
{
    public interface IStage<T> where T : IStep, IStepDetail<IDependency, IParameter>
    {
        string Id { get; }
        List<T> Steps { get; }
    }
}
