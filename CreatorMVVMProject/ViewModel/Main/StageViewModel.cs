using CreatorMVVMProject.Model.Class.Commands;
using CreatorMVVMProject.Model.Class.Main;
using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Interface.ExecutionService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CreatorMVVMProject.ViewModel.Main
{
    public class StageViewModel: INotifyPropertyChanged
    {
        private readonly List<StepViewModel> stepViewModels = new();
        
        public StageViewModel(StageStatus stage, IExecutionService executionService)
        {
            this.Stage = stage;
            StepViewModel stepViewModel;

            foreach (StepStatus step in this.Stage.Steps)
            {
                stepViewModel = new StepViewModel(step, executionService);
                stepViewModels.Add(stepViewModel);
            }
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void NotifyPropertyChange(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StageStatus Stage { get; set; }

        public string StageId => this.Stage.Id;

        public List<StepViewModel> StepViewModels => this.stepViewModels;

    }
    
}
