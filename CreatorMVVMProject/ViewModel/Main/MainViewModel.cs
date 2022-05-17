using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Class.Main;
using System.Collections.Generic;
using System.Windows.Input;
using CreatorMVVMProject.Model.Class.Commands;
using System.Linq;

namespace CreatorMVVMProject.ViewModel.Main
{
    public class MainViewModel
    {
        private readonly MainModel mainModel;
        private List<StageViewModel> stageViewModels = new();
        private StageViewModel selectedStage;

        private ICommand? startExecutionCommand;

        //ovdje trebam imati inject-ovan neki executor kom cu proslijediti listu STEP STATUSA koji idu na izvrsavanje !
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
                //if(this.startExecutionCommand == null)
                //{
                //    this.startExecutionCommand = new DelegateCommand(StartExecutionCommandHandler);
                //}
                return this.startExecutionCommand ??= new DelegateCommand(StartExecutionCommandHandler);
            }
        }

        public void StartExecutionCommandHandler()
        {
            //Odavde saljem stepove koji se "pripremaju" za izvrsavanje

            List<StepStatus> steps = new();
            foreach(StepViewModel stepViewModel in GetSelectedStepViewModels())
                steps.Add(stepViewModel.StepStatus);

            mainModel.AddStepsToExecution(steps);
            ResetCheckBoxes();

            //ResetCheckBoxes();
        }

        private IList<StepViewModel> GetSelectedStepViewModels()
        {
            return this.stageViewModels.SelectMany(stageViewModel => stageViewModel.StepViewModels).Where(stepViewModel => stepViewModel.IsSelected).ToList();
        }

        private void ResetCheckBoxes()
        {
            foreach (StepViewModel stepViewModel in stageViewModels.SelectMany(stageViewModel => stageViewModel.StepViewModels).ToList())
                stepViewModel.IsSelected = false;
        }

    }
}
