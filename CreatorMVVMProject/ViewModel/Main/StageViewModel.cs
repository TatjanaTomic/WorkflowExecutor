using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Interface.ExecutionService;
using System.Collections.Generic;
using System.ComponentModel;

namespace CreatorMVVMProject.ViewModel.Main
{
    public class StageViewModel: INotifyPropertyChanged
    {
        private readonly List<StepViewModel> stepViewModels = new();

        public StageViewModel(StageStatus stage, IExecutionService executionService)
        {
            Stage = stage;
            StepViewModel stepViewModel;

            foreach (StepStatus step in Stage.Steps)
            {
                stepViewModel = new StepViewModel(step, executionService);
                stepViewModels.Add(stepViewModel);
            }
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void NotifyPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StageStatus Stage { get; set; }

        public string StageId => Stage.Id;

        public List<StepViewModel> StepViewModels => stepViewModels;

    }
    
}
