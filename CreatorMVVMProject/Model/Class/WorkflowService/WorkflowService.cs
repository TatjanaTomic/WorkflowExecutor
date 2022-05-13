using CreatorMVVMProject.Model.Interface.WorkflowService;
using ExecutionEngine.Xml;
using System.Collections.Generic;
using System.Linq;

namespace CreatorMVVMProject.Model.Class.WorkflowService
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IList<Stage> stages;
        private readonly List<Step> allSteps = new();

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

        public List<Step> GetFirstLevelDependencySteps(Step step)
        {
            List<Step> dependencySteps = new();

            foreach (var dependency in step.Dependencies)
            {
                Step? dependencyStep = allSteps.Find(s => s.Id == dependency.DependencyStepId);
                if (dependencyStep != null)
                    dependencySteps.Add(dependencyStep);
            }

            return dependencySteps;
        }

        public List<Step> GetAllDependencySteps(Step step)
        {
            List<Step> firstLevelDependencySteps = GetFirstLevelDependencySteps(step);

            List<Step> allDependencySteps = new();
            allDependencySteps.AddRange(firstLevelDependencySteps);

            foreach (Step dependencyStep in firstLevelDependencySteps)
            {
                allDependencySteps.AddRange(GetAllDependencySteps(dependencyStep).Except(allDependencySteps));
            }

            return allDependencySteps;
        }

        private void ReadAllSteps()
        {
            if(this.stages != null)
            {
                foreach(var stage in this.stages)
                {
                    foreach (var step in stage.Steps)
                        this.allSteps.Add(step);
                }
            }
        }
    }
}
