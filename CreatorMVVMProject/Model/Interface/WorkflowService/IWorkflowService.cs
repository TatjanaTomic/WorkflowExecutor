using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using System.Collections.Generic;

namespace CreatorMVVMProject.Model.Interface.WorkflowService
{
    public interface IWorkflowService
    {
        IList<Stage> Stages { get; }
        bool HasDependencySteps(Step step);
        IList<Step> GetFirstLevelDependencySteps(Step step);
        IList<Step> GetAllDependencySteps(Step step);
        IList<Step> GetReverseDependencySteps(Step step);
    }
}
