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

    public interface IStepDetail
    {
        string Id { get; }
        string? ExecutablePath { get; }
        string? File { get; }
        TaskType Type { get; }
        bool CanBeExecutedInParallel { get; }
        string? Description { get; }
        List<Dependency>? Dependencies { get; }
        List<Parameter>? Parameters { get; }
    }
}
