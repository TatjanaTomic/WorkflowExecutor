using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Interface.WorkflowService;
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

        public IList<Step> GetFirstLevelDependencySteps(Step step)
        {
            IList<Step> dependencySteps = new List<Step>();

            foreach (var dependency in step.Dependencies)
            {
                Step dependencyStep = stages.SelectMany(stage => stage.Steps).First(s => s.Id == dependency.DependencyStepId);

                dependencySteps.Add(dependencyStep);
            }

            return dependencySteps;
        }

        //vraca sve stepove od kojih zavisi proslijedjeni step
        public IList<Step> GetAllDependencySteps(Step step)
        {
            IList<Step> firstLevelDependencySteps = GetFirstLevelDependencySteps(step);

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
        public IList<Step> GetReverseDependencySteps(Step step)
        {
            IList<Step> reverseDependencySteps = new List<Step>();
                
            foreach(Step s in stages.SelectMany(stage => stage.Steps))
            {
                if(s.Dependencies.Any(d => d.DependencyStepId == step.Id))
                    reverseDependencySteps.Add(s);
            }

            return reverseDependencySteps;
        }
    }
}
