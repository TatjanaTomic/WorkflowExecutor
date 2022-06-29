using CreatorMVVMProject.Model.Class.Commands;
using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Interface.ExecutionService;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Type = CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml.Type;

namespace CreatorMVVMProject.ViewModel.Main
{
    public class StepViewModel : INotifyPropertyChanged
    {
        private readonly StepStatus stepStatus;
        private readonly IExecutionService executionService;

        private bool isExpanded = true;
        private bool isButtonEnabled = true;
        private bool isSelected;
        private bool isIndeterminate;
        private ICommand? startStepCommand;

        public StepViewModel(StepStatus stepStatus, IExecutionService executionService)
        {
            this.stepStatus = stepStatus;
            this.stepStatus.StatusChanged += OnStatusChanged;
            this.stepStatus.MessageChanged += OnMessageChanged;
            this.stepStatus.CanBeExecutedChanged += OnCanBeExecutedChanged;
            this.executionService = executionService;            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void NotifyPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler? ExecuteTillThisPressed;
        protected virtual void OnExecuteTillThisPressed()
        {
            ExecuteTillThisPressed?.Invoke(this, EventArgs.Empty);
        }

        public StepStatus StepStatus => stepStatus;

        public Status Status => stepStatus.Status;

        public bool CanBeSelected => stepStatus.CanBeExecuted;

        public string StepId => stepStatus.Step.Id;

        public string? StepDescription => stepStatus.Step.Description;

        public Type StepType => stepStatus.Step.Type;

        public ICommand StartStepCommand => startStepCommand ??= new DelegateCommand(StartStepCommandHandler);

        public string Message
        {
            get => stepStatus.StatusMessage;
            set
            {
                stepStatus.StatusMessage = value;
                NotifyPropertyChange(nameof(Message));
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                NotifyPropertyChange(nameof(IsSelected));
            }
        }

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                isExpanded = value;
                NotifyPropertyChange(nameof(IsExpanded));
            }
        }

        public bool IsButtonEnabled
        {
            get => isButtonEnabled;
            set
            {
                isButtonEnabled = value;
                NotifyPropertyChange(nameof(IsButtonEnabled));
            }
        }

        public bool IsIndeterminate
        {
            get => isIndeterminate;
            set
            {
                isIndeterminate = value;
                NotifyPropertyChange(nameof(IsIndeterminate));
            }
        }

        public void StartStepCommandHandler()
        {
            executionService.ExecuteTillThisStep(StepStatus);

            OnExecuteTillThisPressed();
        }

        private void OnStatusChanged(object? _, StatusChangedEventArgs statusChangedEventArgs)
        {
            IsIndeterminate = statusChangedEventArgs.Status == Status.InProgress;
            NotifyPropertyChange(nameof(Status));
        }

        private void OnMessageChanged(object? _, EventArgs _2)
        {
            NotifyPropertyChange(nameof(Message));
        }

        private void OnCanBeExecutedChanged(object? _, EventArgs _2)
        {
            NotifyPropertyChange(nameof(CanBeSelected));
        }
    }
}
