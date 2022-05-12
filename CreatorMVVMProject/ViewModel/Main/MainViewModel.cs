using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Class.Main;
using System.Collections.Generic;
using System.Windows.Input;
using CreatorMVVMProject.Model.Class.Commands;

namespace CreatorMVVMProject.ViewModel.Main
{
    public class MainViewModel
    {
        private readonly MainModel mainModel;
        private List<StageViewModel> stageViewModels = new();
        private StageViewModel selectedStage;

        private ICommand? startExecutionCommand;

        public MainViewModel(MainModel model)
        {
            this.mainModel = model;
            foreach(StageStatus stage in this.mainModel.Stages)
            {
                stageViewModels.Add(new(stage));
            }
            this.selectedStage = stageViewModels[0];
        }

        public List<StageViewModel> StageViewModels
        {
            get => this.stageViewModels;
            set => this.stageViewModels = value;
        }

        public StageViewModel SelectedStage
        {
            get => this.selectedStage;
            set => this.selectedStage = value;
        }

        public ICommand StartExecutionCommand
        {
            get
            {
                if(this.startExecutionCommand == null)
                {
                    this.startExecutionCommand = new DelegateCommand(StartExecutionCommandHandler);
                }
                return this.startExecutionCommand;
            }
        }

        public void StartExecutionCommandHandler()
        {
            Test(GetSelectedStepViewModels());
        }

        private IList<StepViewModel> GetSelectedStepViewModels()
        {
            IList<StepViewModel> selectedSteps = new List<StepViewModel>();
            foreach (StageViewModel stageViewModel in this.stageViewModels)
            {
                foreach (StepViewModel stepViewModel in stageViewModel.StepViewModels)
                    if(stepViewModel.IsSelected == true)
                        //TODO : Dodati provjeru da li je Enabled
                        selectedSteps.Add(stepViewModel);
            }
            return selectedSteps;
        }

        private void Test(IList<StepViewModel> selectedSteps)
        {
            foreach (StepViewModel stepViewModel in selectedSteps)
            {
                if(stepViewModel.StepStatus.Executor != null) 
                { 
                    stepViewModel.StepStatus.Executor.Start();
                    mainModel.DependencySteps(stepViewModel.StepStatus.Step);
                }
                    
            }
        }
    }
}
