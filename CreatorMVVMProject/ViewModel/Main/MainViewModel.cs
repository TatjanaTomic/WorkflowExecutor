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
        }

        private IList<StepViewModel> GetSelectedStepViewModels()
        {
            //IList<StepViewModel> selectedSteps = new List<StepViewModel>();
            //foreach (StageViewModel stageViewModel in this.stageViewModels)
            //{
            //    foreach (StepViewModel stepViewModel in stageViewModel.StepViewModels)
            //        if(stepViewModel.IsSelected == true)
            //            //TODO : Dodati provjeru da li je Enabled
            //            selectedSteps.Add(stepViewModel);
            //}
            //return selectedSteps;
            //var test = this.stageViewModels.Select(x => x.Stage);
            //var test = from stage in this.mainModel.Stages where 
            return this.stageViewModels.SelectMany(stage => stage.StepViewModels).Where(step => step.IsSelected).ToList();
        }
    }
}
