using System;
using System.Windows.Input;

namespace CreatorMVVMProject.Model.Class.Commands;

public class DelegateCommand<T> : ICommand
{
    private readonly Action<T> action;

    public DelegateCommand(Action<T> action)
    {
        this.action = action;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        action((T?)parameter);

    }
}


public class DelegateCommand : ICommand
{
    private readonly Action action;

    public DelegateCommand(Action action)
    {
        this.action = action;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        action();
    }
}
