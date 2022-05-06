using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Step
{
    public interface IStep<Dependency, Parameter> where Dependency : IDependency where Parameter : IParameter
    {
        string Id { get; }
        string? ExecutablePath { get; }
        string? File { get; }
        Type Type { get; }
        bool CanBeExecutedInParallel { get; }
        string? Description { get; }
        List<Dependency>? Dependencies { get; }
        List<Parameter>? Parameters { get; }

        void Execute();
    }

}
