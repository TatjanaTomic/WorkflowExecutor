using CreatorMVVMProject.Model.Interface.WorkflowService;
using ExecutionEngine.Xml;
using System.Collections.Generic;
using System.Linq;

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

        public List<Step> GetFirstLevelDependencySteps(Step step)
        {
            List<Step> dependencySteps = new();

            foreach (var dependency in step.Dependencies)
            {
                Step dependencyStep = stages.SelectMany(stage => stage.Steps).Where(s => s.Id == dependency.DependencyStepId).First();

                dependencySteps.Add(dependencyStep);
            }

            return dependencySteps;
        }

        //vraca sve stepove od kojih zavisi proslijedjeni step
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

        public bool HasDependencySteps(Step step)
        {
            return step.Dependencies.Count > 0;
        }

        //vraca sve stepove koji zavise od proslijedjenog stepa
        // TODO : nadji srecniji naziv
        public List<Step> GetReverseDependencySteps(Step step)
        {
            List<Step> reverseDependencySteps = new();
                
            foreach(Step s in stages.SelectMany(stage => stage.Steps))
            {
                if(s.Dependencies.Any(d => d.DependencyStepId == step.Id))
                    reverseDependencySteps.Add(s);
            }

            return reverseDependencySteps;
        }
    }
}
