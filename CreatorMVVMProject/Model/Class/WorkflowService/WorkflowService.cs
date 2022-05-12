using CreatorMVVMProject.Model.Interface.WorkflowService;
using ExecutionEngine.Xml;
using System.Collections.Generic;

namespace CreatorMVVMProject.Model.Class.WorkflowService
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IList<Stage> stages;
        private readonly List<Step> steps = new();

        private readonly IWorkflowRepository workflowRepository;

        public WorkflowService(IWorkflowRepository workflowRepository)
        {
            this.workflowRepository = workflowRepository;
            this.stages = this.workflowRepository.GetAllStages();
            ReadAllSteps();
        }

        public IList<Stage> Stages
        {
            get => this.stages;
        }

        public IList<Step> GetAllDependencySteps(Step step)
        {
            IList<Step> dependencySteps = new List<Step>();

            foreach(var dependency in step.Dependencies)
            {
                Step? dependencyStep = steps.Find(s => s.Id == dependency.DependencyStep);
                if(dependencyStep != null)
                    dependencySteps.Add(dependencyStep);
            }

            return dependencySteps;
        }

        private void ReadAllSteps()
        {
            if(this.stages != null)
            {
                foreach(var stage in this.stages)
                {
                    foreach (var step in stage.Steps)
                        this.steps.Add(step);
                }
            }

        }
    }
}
