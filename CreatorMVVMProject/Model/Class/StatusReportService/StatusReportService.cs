﻿using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Interface.StatusReportService;
using CreatorMVVMProject.Model.Interface.WorkflowService;
using System.Collections.Generic;
using System.Linq;

namespace CreatorMVVMProject.Model.Class.StatusReportService
{
    public class StatusReportService : IStatusReportService
    {
        private readonly IWorkflowService workflowService;
        private readonly IList<StageStatus> stages = new List<StageStatus>();

        public StatusReportService(IWorkflowService workflowService)
        {
            this.workflowService = workflowService;

            foreach(var stage in workflowService.Stages)
            {
                stages.Add(new(stage, this));
            }
        }

        public IList<StageStatus> Stages => this.stages;

        public Status GetInitialStatus(Step step)
        {
            if (workflowService.HasDependencySteps(step))
                return Status.Disabled;
            
            return Status.NotStarted;
        }

        public bool CanStepBeExecutedInitial(Step step)
        {
            if(workflowService.HasDependencySteps(step))
                return false;

            return true;
        }

        public bool CanStepBeExecuted(StepStatus stepStatus)
        {
            if(stepStatus.Status == Status.Disabled || stepStatus.Status == Status.InProgress)
                return false;

            //TODO : Provjeri ovaj uslov
            // Ako je step dosao do stanja obsolete, da li to znaci da su svi njegovi zavisni sigurno zavrseni i ne moze izvrsiti samo ako je neki od njegovih zavisnih Failed ?
            //if (stepStatus.Status == Status.Obsolete && workflowService.GetAllDependencySteps(stepStatus.Step).Exists(s => GetStepStatus(s).Status == Status.Failed))
            //    return false;
            
            if (stepStatus.Status == Status.Obsolete && workflowService.GetAllDependencySteps(stepStatus.Step).ToList().Exists(s => GetStepStatus(s).Status != Status.Success))
                return false;
            
            return true;
        }

        public void SetStatusToStep(StepStatus stepStatus, Status status) 
        {
            Status oldStatus = stepStatus.Status;
            stepStatus.Status = status; //promijeni odmah status na novi

            if(status == Status.Success)
            {
                IList<StepStatus> reverseDependencySteps = GetStepStatuses(workflowService.GetReverseDependencySteps(stepStatus.Step).ToList());

                foreach (StepStatus dependencyStepStatus in reverseDependencySteps)
                {
                    IList<StepStatus> firstLevelDependencySteps = GetStepStatuses(workflowService.GetFirstLevelDependencySteps(dependencyStepStatus.Step).ToList());  

                    if(dependencyStepStatus.Status == Status.Disabled && firstLevelDependencySteps.All(s => s.Status == Status.Success) )
                    {
                        SetStatusToStep(dependencyStepStatus, Status.NotStarted);
                    }
                }
            }

            if (oldStatus.Equals(Status.Success) && status.Equals(Status.InProgress))
            {
                IList<Step> obsoletedSteps = workflowService.GetAllDependencySteps(stepStatus.Step);
                foreach(Step step in obsoletedSteps)
                {
                    SetStatusToStep(step, Status.Obsolete);
                }
            }

            stepStatus.CanBeExecuted = CanStepBeExecuted(stepStatus);
        }

        public void SetStatusToStep(Step step, Status status)
        {
            StepStatus stepStatus = GetStepStatus(step);
            SetStatusToStep(stepStatus, status);
        }

        public void SetStatusMessageToStep(Step step, string message)
        {
            StepStatus stepStatus = GetStepStatus(step);
            stepStatus.StatusMessage = message;
        }

        public StepStatus GetStepStatus(Step step)
        {
            return stages.SelectMany(stage => stage.Steps).Where(s => s.Step.Id == step.Id).First();
        }

        public IList<StepStatus> GetStepStatuses(List<Step> steps)
        {
            IList<StepStatus> stepStatuses = new List<StepStatus>();
            foreach (Step step in steps)
            {
                stepStatuses.Add(GetStepStatus(step));
            }
            return stepStatuses;
        }
    }
}
