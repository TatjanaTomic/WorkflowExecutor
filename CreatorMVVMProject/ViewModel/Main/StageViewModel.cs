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
        private StageStatus stage;
        private readonly List<StepViewModel> stepViewModels = new();
        
        public StageViewModel(StageStatus stage, IExecutionService executionService)
        {
            this.stage = stage;
            StepViewModel stepViewModel;

            foreach (StepStatus step in this.stage.Steps)
            {
                stepViewModel = new StepViewModel(step, executionService);
                stepViewModels.Add(stepViewModel);
                //stepViewModel += ExecuteTillThisPressed;
            }
        }

        public StageStatus Stage
        {
            get => this.stage; 
            set => this.stage = value;
        }
        
        public string StageId
        {
            get => this.stage.Id;
        }

        public List<StepViewModel> StepViewModels
        {
            get => this.stepViewModels;
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void NotifyPropertyChange(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ExecuteTillThisPressed()
        {

        }
    }
    
}
