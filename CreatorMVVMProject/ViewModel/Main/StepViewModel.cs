using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using CreatorMVVMProject.Model.Class.Commands;
using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Interface.ExecutionService;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using CreatorMVVMProject.Model.Interface.WorkflowService;
using Type = CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml.Type;

namespace CreatorMVVMProject.ViewModel.Main
{
    public class StepViewModel : INotifyPropertyChanged
    {
        private readonly StepStatus stepStatus;
        private readonly IExecutionService executionService;
        private readonly IStatusReportService statusReportService;
        private readonly IWorkflowService workflowService;

        private bool isExpanded = true;
        private bool isButtonEnabled = true;
        private bool isSelected;
        private bool isIndeterminate;
        private ICommand? startStepCommand;

        public StepViewModel(StepStatus stepStatus, IExecutionService executionService, IStatusReportService statusReportService, IWorkflowService workflowService)
        {
            this.stepStatus = stepStatus;
            this.stepStatus.StatusChanged += OnStatusChanged;
            this.stepStatus.MessageChanged += OnMessageChanged;
            this.stepStatus.CanBeExecutedChanged += OnCanBeExecutedChanged;
            this.executionService = executionService;
            this.statusReportService = statusReportService;
            this.workflowService = workflowService;

            GenerateTreeView();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ExecuteTillThisPressed;

        public StepStatus StepStatus => stepStatus;

        public Status Status => stepStatus.Status;

        public bool CanBeSelected => stepStatus.CanBeExecuted;

        public string StepId => stepStatus.Step.Id;

        public string? StepDescription => stepStatus.Step.Description;

        public Type StepType => stepStatus.Step.Type;

        public List<TreeViewNode> TreeView { get; set; } = new();

        public ICommand StartStepCommand => startStepCommand ??= new DelegateCommand(StartStepCommandHandler);

        public string Message
        {
            get => stepStatus.StatusMessage;
            set
            {
                stepStatus.StatusMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                isExpanded = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExpanded)));
            }
        }

        public bool IsButtonEnabled
        {
            get => isButtonEnabled;
            set
            {
                isButtonEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsButtonEnabled)));
            }
        }

        public bool IsIndeterminate
        {
            get => isIndeterminate;
            set
            {
                isIndeterminate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsIndeterminate)));
            }
        }

        public void StartStepCommandHandler()
        {
            executionService.ExecuteTillThisStep(StepStatus);

            ExecuteTillThisPressed?.Invoke(this, EventArgs.Empty);
        }

        private void OnStatusChanged(object? _, StatusChangedEventArgs statusChangedEventArgs)
        {
            IsIndeterminate = statusChangedEventArgs.Status == Status.Running;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
        }

        private void OnMessageChanged(object? _, EventArgs _2)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
        }

        private void OnCanBeExecutedChanged(object? _, EventArgs _2)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanBeSelected)));
        }

        private void GenerateTreeView()
        {
            TreeViewNode root = new TreeViewNode(stepStatus);
            AddTreeViewNodes(root);
            TreeView = new List<TreeViewNode> { root };
        }

        private void AddTreeViewNodes(TreeViewNode root)
        {
            List<TreeViewNode> nodes = new();

            IList<StepStatus> dependencies = statusReportService.GetStepStatuses(workflowService.GetFirstLevelDependencySteps(root.StepStatus.Step));
            foreach (StepStatus dependencyStep in dependencies)
            {
                TreeViewNode node = new TreeViewNode(dependencyStep);
                AddTreeViewNodes(node);
                nodes.Add(node);
            }

            root.Children = nodes;
        }
    }

    public class TreeViewNode : INotifyPropertyChanged
    {
        private readonly StepStatus stepStatus;

        public TreeViewNode(StepStatus stepStatus)
        {
            this.stepStatus = stepStatus;
            this.stepStatus.StatusChanged += StepStatus_StatusChanged;
        }

        public StepStatus StepStatus => stepStatus;
        public string NodeStepId => stepStatus.Step.Id;
        public Status NodeStepStatus => stepStatus.Status;
        public List<TreeViewNode> Children { get; set; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        private void StepStatus_StatusChanged(object? sender, StatusChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NodeStepStatus)));
        }
    }
}
