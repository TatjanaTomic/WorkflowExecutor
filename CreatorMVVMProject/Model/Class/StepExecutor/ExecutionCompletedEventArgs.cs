using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;

namespace CreatorMVVMProject.Model.Class.StepExecutor;

public class ExecutionCompletedEventArgs
{
    public ExecutionCompletedEventArgs(Step step, bool isSuccessful, string message)
    {
        Step = step;
        IsSuccessful = isSuccessful;
        Message = message;
    }

    public Step Step { get; set; }
    public bool IsSuccessful { get; set; }
    public string Message { get; set; }
}
