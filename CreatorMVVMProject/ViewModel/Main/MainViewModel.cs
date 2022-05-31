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
        private List<StageViewModel> stageViewModels = new();
        private StageViewModel selectedStage;

        private bool canExecutionStart = true;
        private ICommand? startExecutionCommand;

        public MainViewModel(MainModel model)
        {
            this.mainModel = model;
            this.mainModel.ExecutionCompleted += MainModel_ExecutionCompleted;
            this.mainModel.ExecutionSelectedStepsStarted += MainModel_ExecutionSelectedStepsStarted;
            this.mainModel.ExecutionTillThisStepStarted += MainModel_ExecutionTillThisStepStarted;

            foreach(StageStatus stage in mainModel.Stages)
            {
                stageViewModels.Add(new(stage, this.mainModel.ExecutionService));
            }
            this.selectedStage = stageViewModels[0];
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
            foreach (StepViewModel stepViewModel in stageViewModels.SelectMany(stageViewModel => stageViewModel.StepViewModels).ToList())
                stepViewModel.IsButtonEnabled = true;

            CanExecutionStart = true;
        }

        private void DisableExecuteTillThisButtons()
        {
            foreach (StepViewModel stepViewModel in stageViewModels.SelectMany(stageViewModel => stageViewModel.StepViewModels).ToList())
                stepViewModel.IsButtonEnabled = false;
        }

        private IList<StepViewModel> GetSelectedStepViewModels()
        {
            return this.stageViewModels.SelectMany(stageViewModel => stageViewModel.StepViewModels).Where(stepViewModel => stepViewModel.IsSelected).ToList();
        }

    }
}
