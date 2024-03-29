﻿namespace CreatorMVVMProject.Model.Class.ExecutionService;

public class ExecutionEventArgs
{
    public ExecutionEventArgs(string message, bool executionFailed)
    {
        Message = message;
        ExecutionFailed = executionFailed;
    }

    public string Message { get; set; }

    public bool ExecutionFailed { get; set; }
}
