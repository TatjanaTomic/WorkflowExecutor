using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CreatorMVVMProject.Model.Class.Commands;
using CreatorMVVMProject.Model.Class.ExecutionService;
using CreatorMVVMProject.Model.Class.Main;
using CreatorMVVMProject.Model.Class.StatusReportService;

namespace CreatorMVVMProject.ViewModel.Main
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly MainModel mainModel;

        private StageViewModel? selectedStage;
        private int selectedStageIndex;
        private bool canExecutionStart = true;
        private ICommand? startExecutionCommand;

        public MainViewModel(MainModel model)
        {
            mainModel = model;
            mainModel.ExecutionCompleted += MainModel_ExecutionCompleted;
            mainModel.ExecutionSelectedStepsStarted += MainModel_ExecutionSelectedStepsStarted;
            mainModel.ExecutionTillThisStepStarted += MainModel_ExecutionTillThisStepStarted;

            foreach (StageStatus stage in mainModel.Stages)
            {
                StageViewModels.Add(new(stage, mainModel.ExecutionService, mainModel.StatusReportService, mainModel.WorkflowService));
            }

            setSelectedStage();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<StageViewModel> StageViewModels { get; set; } = new();

        public ICommand StartExecutionCommand => startExecutionCommand ??= new DelegateCommand(StartExecutionCommandHandler);

        public StageViewModel? SelectedStage
        {
            get => selectedStage;
            set
            {
                selectedStage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStage)));
            }
        }

        public int SelectedStageIndex
        {
            get => selectedStageIndex;
            set
            {
                selectedStageIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStageIndex)));
            }
        }

        public bool CanExecutionStart
        {
            get => canExecutionStart;
            set
            {
                canExecutionStart = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanExecutionStart)));
            }
        }

        public void StartExecutionCommandHandler()
        {
            List<StepViewModel> selectedStepViewModels = GetSelectedStepViewModels().ToList();

            List<StepStatus> steps = new();
            foreach (StepViewModel stepViewModel in selectedStepViewModels)
            {
                steps.Add(stepViewModel.StepStatus);
                stepViewModel.IsSelected = false;
            }

            mainModel.AddStepsToExecution(steps);
        }

        private void setSelectedStage()
        {
            if (StageViewModels.Count > 0)
            {
                selectedStage = StageViewModels[0];
                selectedStageIndex = 0;
            }
            else
            {
                mainModel.DialogService.ShowMessage(new Message.MessageViewModel("Unable to start application!", true));
                Application.Current.Shutdown();
            }
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

        private void MainModel_ExecutionTillThisStepStarted(object? sender, ExecutionEventArgs args)
        {
            DisableExecuteTillThisButtons();
            CanExecutionStart = false;

            if (!string.IsNullOrEmpty(args.Message))
            {
                mainModel.DialogService.ShowMessage(new Message.MessageViewModel(args.Message, args.ExecutionFailed));
            }
        }

        private void MainModel_ExecutionCompleted(object? sender, ExecutionEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Message))
            {
                mainModel.DialogService.ShowMessage(new Message.MessageViewModel(args.Message, args.ExecutionFailed));
            }

            EnableButtons();
        }

        private void MainModel_ExecutionSelectedStepsStarted(object? sender, ExecutionEventArgs args)
        {
            DisableExecuteTillThisButtons();

            if (!string.IsNullOrEmpty(args.Message))
            {
                CanExecutionStart = false;
                mainModel.DialogService.ShowMessage(new Message.MessageViewModel(args.Message, args.ExecutionFailed));
            }
        }
    }
}
