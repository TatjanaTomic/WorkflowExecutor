using System.Collections.Generic;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository;

public interface IWorkflowRepository
{
    IList<Stage> GetAllStages();
}
