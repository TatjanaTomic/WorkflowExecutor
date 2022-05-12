using CreatorMVVMProject.Model.Class.Commands;
using CreatorMVVMProject.Model.Class.StatusReportService;
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

        //private ObservableCollection<StepViewModel> selectedSteps = new();
        //private ICommand? startStageCommand;
        
        public StageViewModel(StageStatus stage)
        {
            this.stage = stage;
            StepViewModel stepViewModel;

            foreach (StepStatus step in this.stage.Steps)
            {
                stepViewModel = new StepViewModel(step);
                stepViewModels.Add(stepViewModel);
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
        
        /*
        public ICommand? StartStageCommand
        {
            get
            {
                if(this.startStageCommand == null)
                {
                    this.startStageCommand = new DelegateCommand(StartStageCommandHandler);
                }
                return this.startStageCommand;
            }
        }
        public void StartStageCommandHandler()
        {
            foreach (StepViewModel stepViewModel in this.stepViewModels)
            {
                if (stepViewModel.IsSelected == true)
                //TODO : Dodati u listu selektovane stepove i zapoceti izvrsavanje
            }
        }
        */


        protected virtual void NotifyPropertyChange(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
    
}
