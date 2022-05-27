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
        private bool canBeSelected = true;
        private bool isButtonEnabled = true;

        private IExecutionService executionService;

        private ICommand? startStepCommand;

        public StepViewModel(StepStatus stepStatus, IExecutionService executionService)
        {
            this.stepStatus = stepStatus;
            this.stepStatus.StatusChanged += OnStatusChanged;
            this.stepStatus.MessageChanged += OnMessageChanged;

            this.executionService = executionService;

            if(stepStatus.Status == Status.Disabled)
                canBeSelected = false;
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
        
        public bool CanBeSelected
        {
            get => canBeSelected;
            set
            {
                canBeSelected = value;
                NotifyPropertyChange(nameof(CanBeSelected));
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

        public Status Status
        {
            get => stepStatus.Status;
        }

        public StepStatus StepStatus
        {
            get => this.stepStatus;
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
            //TODO : Kako odavde da za sve stepViewModele promijenim IsButtonEnabled? Da li da pravim neki event?
            //TODO : Ovdje treba da disable-ujem sve ExecuteTillThisStep button-e i Start execution button
            executionService.StartExecuteTillThisStep(StepStatus);
        }

        private void OnStatusChanged(object? _, StatusChangedEventArgs statusChangedEventArgs)
        {
            if (statusChangedEventArgs.Status != Status.InProgress)
                CanBeSelected = true;

            NotifyPropertyChange(nameof(this.Status));
        }

        private void OnMessageChanged(object? _, EventArgs _2)
        {
            NotifyPropertyChange(nameof(this.Message));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void NotifyPropertyChange(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
