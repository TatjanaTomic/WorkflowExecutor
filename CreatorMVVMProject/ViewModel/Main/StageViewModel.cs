using CreatorMVVMProject.Model.Class.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Interface.ExecutionService;
using System.Windows.Input;

namespace CreatorMVVMProject.ViewModel.Main
{
    public class StageViewModel
    {
        private DelegateCommand? expandAllCommand;
        private DelegateCommand? collapseAllCommand;

        public StageViewModel(StageStatus stage, IExecutionService executionService)
        {
            Stage = stage;

            foreach (StepStatus step in Stage.Steps)
            {
                StepViewModels.Add(new StepViewModel(step, executionService));
            }
        }

        public StageStatus Stage { get; set; }

        public string StageId => Stage.Id;

        public List<StepViewModel> StepViewModels { get; } = new();

        public ICommand ExpandAllCommand => expandAllCommand ??= new DelegateCommand(ExpandAll);
        
        public ICommand CollapseAllCommand => collapseAllCommand ??= new DelegateCommand(CollapseAll);

        private void ExpandAll()
        {
            foreach (StepViewModel stepViewModel in StepViewModels)
            {
                stepViewModel.IsExpanded = true;
            }
        }
        
        private void CollapseAll()
        {
            foreach (StepViewModel stepViewModel in StepViewModels)
            {
                stepViewModel.IsExpanded = false;
            }
        }
    }

}
