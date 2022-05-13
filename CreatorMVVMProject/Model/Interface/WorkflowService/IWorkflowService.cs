﻿using CreatorMVVMProject.Model.Class.WorkflowService;
using ExecutionEngine.Xml;
using System.Collections.Generic;

namespace CreatorMVVMProject.Model.Interface.WorkflowService
{
    public interface IWorkflowService
    {
        IList<Stage> Stages
        {
            get;
        }

        List<Step> GetFirstLevelDependencySteps(Step step);
        List<Step> GetAllDependencySteps(Step step);
    }
}
