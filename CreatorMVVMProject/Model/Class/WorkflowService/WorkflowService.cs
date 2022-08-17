using System.Collections.Generic;
using System.Linq;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Interface.WorkflowService;

namespace CreatorMVVMProject.Model.Class.WorkflowService
{
    /// <summary>
    /// Class <c>WorkflowService</c> models a service that reads Steps from specified repository and is in charge of calculating interdependencies of given Steps.
    /// </summary>
    public class WorkflowService : IWorkflowService
    {
        public WorkflowService(IWorkflowRepository workflowRepository)
        {
            Stages = workflowRepository.GetAllStages();
        }

        public IList<Stage> Stages { get; }

        public bool HasDependencySteps(Step step)
        {
            return step.Dependencies.Count > 0;
        }

        /// <summary>
        /// Method <c>GetFirstLevelDependencySteps</c> calculates the list of steps on which the forwarded step directly depends.
        /// </summary>
        /// <param name="step">Method takes a step for which calculates first level dependency steps.</param>
        /// <returns>Method returns a list of dependency steps. List is empty if the given step has no dependency steps.</returns>
        public IList<Step> GetFirstLevelDependencySteps(Step step)
        {
            IList<string> dependencySteps = step.Dependencies.Select(x => x.DependencyStepId).ToList();

            return Stages.SelectMany(stage => stage.Steps).Where(x => dependencySteps.Contains(x.Id)).ToList();
        }

        /// <summary>
        /// Method <c>GetAllDependencySteps</c> calculates the list of all steps on which the forwarded step depends.
        /// </summary>
        /// <param name="step">Method takes a step for which calculates all dependency steps.</param>
        /// <returns>Method returns a list of dependency steps. List is empty if given step has no dependency steps.</returns>
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

        /// <summary>
        /// Method <c>GetReverseDependencySteps</c> calculates the list of all steps that depend on a given step.
        /// </summary>
        /// <param name="step">Method takes a step for which calculates reverse dependency steps.</param>
        /// <returns>Method returns a list of dependency steps. List is empty if there is no step that depends on given step.</returns>
        public IList<Step> GetReverseDependencySteps(Step step)
        {
            return Stages.SelectMany(stage => stage.Steps).Where(s => s.Dependencies.Any(d => d.DependencyStepId == step.Id)).ToList();
        }
    }
}
