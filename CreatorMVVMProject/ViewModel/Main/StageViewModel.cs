using System.Collections.Generic;
using System.ComponentModel;
using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Interface.ExecutionService;

namespace CreatorMVVMProject.ViewModel.Main
{
    public class StageViewModel : INotifyPropertyChanged
    {
        public StageViewModel(StageStatus stage, IExecutionService executionService)
        {
            Stage = stage;

            foreach (StepStatus step in Stage.Steps)
            {
                StepViewModels.Add(new StepViewModel(step, executionService));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void NotifyPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public StageStatus Stage { get; set; }

        public string StageId => Stage.Id;

        public List<StepViewModel> StepViewModels { get; } = new();

    }

}
