using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using System.Collections.Generic;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository
{
    public interface IWorkflowRepository
    {
        IList<Stage> GetAllStages();
    }
}