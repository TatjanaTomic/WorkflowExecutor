using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Class.Main;
using System.Collections.Generic;
using System.Windows.Input;
using CreatorMVVMProject.Model.Class.Commands;
using System.Linq;
using System.ComponentModel;

namespace CreatorMVVMProject.ViewModel.Main
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly MainModel mainModel;
        private List<StageViewModel> stageViewModels = new();
        private StageViewModel selectedStage;

        private bool canExecutionStart = true;
        private ICommand? startExecutionCommand;

        public MainViewModel(MainModel model)
        {
            this.mainModel = model;
            foreach(StageStatus stage in mainModel.Stages)
            {
                stageViewModels.Add(new(mainModel, stage));
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

        public bool CanExecutionStart
        {
            get => this.canExecutionStart;
            set
            {
                this.canExecutionStart = value;
                NotifyPropertyChange(nameof(CanExecutionStart));
            }
        }
        public ICommand StartExecutionCommand
        {
            get
            {
                return this.startExecutionCommand ??= new DelegateCommand(StartExecutionCommandHandler);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void NotifyPropertyChange(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void StartExecutionCommandHandler()
        {
            //Odavde saljem stepove koji se "pripremaju" za izvrsavanje

            CanExecutionStart = false;

            List<StepStatus> steps = new();
            foreach(StepViewModel stepViewModel in GetSelectedStepViewModels())
                steps.Add(stepViewModel.StepStatus);

            mainModel.AddStepsToExecution(steps);
            ResetCheckBoxes();

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
