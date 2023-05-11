using System;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;

namespace CreatorMVVMProject.Model.Class.StatusReportService;

public class StepStatus
{
    private Status status;
    private bool canBeExecuted;
    private string statusMessage = string.Empty;

    public StepStatus(Step step, Status initialStatus, bool canBeExecuted)
    {
        Step = step;
        status = initialStatus;
        this.canBeExecuted = canBeExecuted;
    }

    public event EventHandler? MessageChanged;

    public event EventHandler? CanBeExecutedChanged;

    public event EventHandler<StatusChangedEventArgs>? StatusChanged;

    public Step Step { get; }

    public string StatusMessage
    {
        get => statusMessage;
        set
        {
            statusMessage = value;
            MessageChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public Status Status
    {
        get => status;
        set
        {
            status = value;
            StatusChanged?.Invoke(this, new StatusChangedEventArgs(status, Step.Id));
        }
    }

    public bool CanBeExecuted
    {
        get => canBeExecuted;
        set
        {
            canBeExecuted = value;
            CanBeExecutedChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
