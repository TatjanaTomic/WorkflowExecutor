using CreatorMVVMProject.Model.Interface.WorkflowService;
using ExecutionEngine.Xml;
using System.Collections.Generic;

namespace CreatorMVVMProject.Model.Class.WorkflowService
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IList<Stage> stages;
        private readonly IWorkflowRepository workflowRepository;

        public WorkflowService(IWorkflowRepository workflowRepository)
        {
            this.workflowRepository = workflowRepository;
            this.stages = this.workflowRepository.GetAllStages();
        }

        public IList<Stage> Stages
        {
            get => this.stages;
        }
    }
}
