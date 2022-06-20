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
        protected readonly StepStatus stepStatus;

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
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler? ExecuteTillThisPressed;
        protected virtual void OnExecuteTillThisPressed()
        {
            ExecuteTillThisPressed?.Invoke(this, EventArgs.Empty);
        }

        public StepStatus StepStatus => stepStatus;

        public Status Status => stepStatus.Status;

        public bool CanBeSelected => this.stepStatus.CanBeExecuted;

        public string StepId => this.stepStatus.Step.Id;

        public string? StepDescription => this.stepStatus.Step.Description;

        public Type StepType => this.stepStatus.Step.Type;

        public string Message
        {
            get => this.stepStatus.StatusMessage;
            set
            {
                this.stepStatus.StatusMessage = value;
                NotifyPropertyChange(nameof(Message));
            }
        }

        public bool IsSelected
        {
            get => this.isSelected;
            set
            {
                this.isSelected = value;
                NotifyPropertyChange(nameof(IsSelected));
            }
        }

        public bool IsExpanded
        {
            get => this.isExpanded;
            set
            {
                this.isExpanded = value;
                NotifyPropertyChange(nameof(IsExpanded));
            }
        }

        public bool IsButtonEnabled
        {
            get => this.isButtonEnabled;
            set
            {
                this.isButtonEnabled = value;
                NotifyPropertyChange(nameof(IsButtonEnabled));
            }
        }

        public bool IsIndeterminate
        {
            get => this.isIndeterminate;
            set
            {
                this.isIndeterminate = value;
                NotifyPropertyChange(nameof(IsIndeterminate));
            }
        }

        public ICommand StartStepCommand => this.startStepCommand ??= new DelegateCommand(StartStepCommandHandler);

        public void StartStepCommandHandler()
        {
            executionService.ExecuteTillThisStep(StepStatus);

            OnExecuteTillThisPressed();
        }

        private void OnStatusChanged(object? _, StatusChangedEventArgs statusChangedEventArgs)
        {
            IsIndeterminate = statusChangedEventArgs.Status == Status.InProgress;
            NotifyPropertyChange(nameof(this.Status));
        }

        private void OnMessageChanged(object? _, EventArgs _2)
        {
            NotifyPropertyChange(nameof(this.Message));
        }

        private void OnCanBeExecutedChanged(object? _, EventArgs _2)
        {
            NotifyPropertyChange(nameof(this.CanBeSelected));
        }
    }
}
