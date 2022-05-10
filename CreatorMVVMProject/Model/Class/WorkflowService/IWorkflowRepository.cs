using ExecutionEngine.Xml;
using System.Collections.Generic;

namespace CreatorMVVMProject.Model.Class.WorkflowService
{
    public interface IWorkflowRepository
    {
        IList<Stage> GetAllStages();
    }
}