﻿using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorMVVMProject.Model.Interface.StatusReportService
{
    public interface IStatusReportService
    {
        IList<StageStatus> Stages { get; }
        void SetStatusToStep(StepStatus stepStatus, Status status);
        void SetStatusToStep(Step step, Status status);
        void SetStatusMessageToStep(Step step, string message);
        Status GetInitialStatus(Step step);
        StepStatus GetStepStatus(Step step);
        IList<StepStatus> GetStepStatuses(List<Step> steps);
        void SetCanStepBeExecuted(StepStatus stepStatus);
        bool CanStepBeExecutedInitial(Step step);
    }
}
