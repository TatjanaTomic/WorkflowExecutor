using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
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
        bool HasDependencySteps(Step step);
        List<Step> GetReverseDependencySteps(Step step);
    }
}
