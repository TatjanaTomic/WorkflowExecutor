using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Step
{
    public enum TaskType
    {
        Executable,
        Upload,
        Download
    }

    public interface IStep<D, P> where D : IDependency where P : IParameter
    {
        string Id { get; }
        string? ExecutablePath { get; }
        string? File { get; }
        TaskType Type { get; }
        bool CanBeExecutedInParallel { get; }
        string? Description { get; }
        List<D>? Dependencies { get; }
        List<P>? Parameters { get; }

        void Execute();
    }
}
