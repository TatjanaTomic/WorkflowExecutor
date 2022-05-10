using CreatorMVVMProject.Model.Class.Commands;
using CreatorMVVMProject.Model.Class.Converters;
using CreatorMVVMProject.Model.Class.StatusReportService;
using ExecutionEngine.Step;
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
        
        private ICommand? startStepCommand;

        public StepViewModel(StepStatus stepStatus)
        {
            this.stepStatus = stepStatus;
            this.stepStatus.StatusChanged += OnStatusChanged;
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

        public Status Status
        {
            get => stepStatus.Status;
        }

        public ICommand StartStepCommand
        {
            get
            {
                if (this.startStepCommand == null)
                {
                    this.startStepCommand = new DelegateCommand(StartStepCommandHandler);
                }
                return this.startStepCommand;
            }
        }

        public void StartStepCommandHandler()
        {
            if(stepStatus.Executor != null)
                stepStatus.Executor.Start();
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
    }
}
