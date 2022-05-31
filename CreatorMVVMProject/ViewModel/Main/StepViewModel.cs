using CreatorMVVMProject.Model.Class.Commands;
using CreatorMVVMProject.Model.Class.Main;
using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Interface.ExecutionService;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace CreatorMVVMProject.ViewModel.Main
{
    public class StepViewModel : INotifyPropertyChanged
    {
        protected readonly StepStatus stepStatus;

        private bool isSelected = false;
        private bool isExpanded = true;
        private bool isButtonEnabled = true;

        private ICommand? startStepCommand;

        private readonly IExecutionService executionService;

        public StepViewModel(StepStatus stepStatus, IExecutionService executionService)
        {
            this.stepStatus = stepStatus;
            this.stepStatus.StatusChanged += OnStatusChanged;
            this.stepStatus.MessageChanged += OnMessageChanged;
            this.stepStatus.CanBeExecutedChanged += OnCanBeExecutedChanged;

            this.executionService = executionService;
        }

        public StepStatus StepStatus { get { return stepStatus; } }

        public Status Status
        {
            get => stepStatus.Status;
        }

        public bool CanBeSelected
        {
            get => this.stepStatus.CanBeExecuted;
        }

        public string StepId
        {
            get
            {
                return this.stepStatus.Step.Id;
            }
        }

        public string? StepDescription
        {
            get 
            {
                return this.stepStatus.Step.Description;
            }
        }

        public string Message
        {
            get { return this.stepStatus.StatusMessage; }
            set
            {
                this.stepStatus.StatusMessage = value;
                NotifyPropertyChange(nameof(Message));
            }
        }

        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                NotifyPropertyChange(nameof(IsSelected));
            }
        }

        public bool IsExpanded
        {
            get { return this.isExpanded; }
            set { 
                this.isExpanded = value;
                NotifyPropertyChange(nameof(IsExpanded));
            }
        }

        public bool IsButtonEnabled
        {
            get { return this.isButtonEnabled; }
            set
            {
                this.isButtonEnabled = value;
                NotifyPropertyChange(nameof(IsButtonEnabled));
            }
        }

        public ICommand StartStepCommand
        {
            get
            {
                return this.startStepCommand ??= new DelegateCommand(StartStepCommandHandler);
            }
        }

        public void StartStepCommandHandler()
        {
            executionService.ExecuteTillThisStep(StepStatus);

            OnExecuteTillThisPressed();
        }

        private void OnStatusChanged(object? _, StatusChangedEventArgs statusChangedEventArgs)
        {
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
    }
}
