using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Class.Main;
using System.Collections.Generic;
using System.Windows.Input;
using CreatorMVVMProject.Model.Class.Commands;
using System.Linq;
using System.ComponentModel;
using System.Windows;
using System;

namespace CreatorMVVMProject.ViewModel.Main
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly MainModel mainModel;

        private StageViewModel selectedStage;
        private int selectedStageIndex;
        private bool canExecutionStart = true;
        private ICommand? startExecutionCommand;

        public MainViewModel(MainModel model)
        {
            mainModel = model;
            mainModel.ExecutionCompleted += MainModel_ExecutionCompleted;
            mainModel.ExecutionSelectedStepsStarted += MainModel_ExecutionSelectedStepsStarted;
            mainModel.ExecutionTillThisStepStarted += MainModel_ExecutionTillThisStepStarted;

            foreach(StageStatus stage in mainModel.Stages)
            {
                StageViewModels.Add(new(stage, mainModel.ExecutionService));
            }
            selectedStage = StageViewModels[0];
            selectedStageIndex = 0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void NotifyPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<StageViewModel> StageViewModels { get; set; } = new();

        public ICommand StartExecutionCommand => startExecutionCommand ??= new DelegateCommand(StartExecutionCommandHandler);

        public StageViewModel SelectedStage
        {
            get => selectedStage;
            set
            {
                selectedStage = value;
                NotifyPropertyChange(nameof(SelectedStage));
            }
        }

        public int SelectedStageIndex
        {
            get => selectedStageIndex;
            set
            {
                selectedStageIndex = value;
                NotifyPropertyChange(nameof(SelectedStageIndex));
            }
        }

        public bool CanExecutionStart
        {
            get => canExecutionStart;
            set
            {
                canExecutionStart = value;
                NotifyPropertyChange(nameof(CanExecutionStart));
            }
        }

        public void StartExecutionCommandHandler()
        {
            List<StepViewModel> selectedStepViewModels = GetSelectedStepViewModels().ToList();
            if(selectedStepViewModels.Count == 0)
            {
                _ = MessageBox.Show("Select steps for execution");
                return;
            }

            List<StepStatus> steps = new();
            foreach(StepViewModel stepViewModel in selectedStepViewModels)
            {
                steps.Add(stepViewModel.StepStatus);
                stepViewModel.IsSelected = false;
            }

            mainModel.AddStepsToExecution(steps);
        }

        private void EnableButtons()
        {
            foreach (StepViewModel stepViewModel in StageViewModels.SelectMany(stageViewModel => stageViewModel.StepViewModels).ToList())
            {
                stepViewModel.IsButtonEnabled = true;
            }

            CanExecutionStart = true;
        }

        private void DisableExecuteTillThisButtons()
        {
            foreach (StepViewModel stepViewModel in StageViewModels.SelectMany(stageViewModel => stageViewModel.StepViewModels).ToList())
            {
                stepViewModel.IsButtonEnabled = false;
            }
        }

        private IList<StepViewModel> GetSelectedStepViewModels()
        {
            return StageViewModels.SelectMany(stageViewModel => stageViewModel.StepViewModels).Where(stepViewModel => stepViewModel.IsSelected).ToList();
        }

        private void MainModel_ExecutionTillThisStepStarted(object? sender, EventArgs e)
        {
            DisableExecuteTillThisButtons();
            CanExecutionStart = false;
        }

        private void MainModel_ExecutionCompleted(object? sender, EventArgs e)
        {
            EnableButtons();
        }

        private void MainModel_ExecutionSelectedStepsStarted(object? sender, EventArgs e)
        {
            DisableExecuteTillThisButtons();
        }
    }
}
