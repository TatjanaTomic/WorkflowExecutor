using CreatorMVVMProject.Model.Class.Commands;
using CreatorMVVMProject.Model.Class.Converters;
using CreatorMVVMProject.Model.Class.Main;
using CreatorMVVMProject.Model.Class.StatusReportService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CreatorMVVMProject.ViewModel.Main
{
    public class StepViewModel : INotifyPropertyChanged
    {
        protected readonly StepStatus stepStatus;

        private readonly MainModel mainModel;

        private bool isSelected = false;
        private bool isExpanded = true;

        private ICommand? startStepCommand;

        public StepViewModel(MainModel mainModel, StepStatus stepStatus)
        {
            this.mainModel = mainModel;
            this.stepStatus = stepStatus;
            this.stepStatus.StatusChanged += OnStatusChanged;
            this.stepStatus.MessageChanged += OnMessageChanged;
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
            mainModel.ExecuteTillThisStep(this.stepStatus);
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void NotifyPropertyChange(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnStatusChanged(object? _, StatusChangedEventArgs _2)
        {
            NotifyPropertyChange(nameof(this.Status));
        }

        private void OnMessageChanged(object? _, StatusChangedEventArgs _2)
        {
            NotifyPropertyChange(nameof(this.Message));
        }
    }
}
