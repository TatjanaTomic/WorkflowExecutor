using System;
using System.Collections.Generic;
using System.Configuration;
using CreatorMVVMProject.Model.Class.Exceptions;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml.StageListBuilder;
using ConfigurationException = CreatorMVVMProject.Model.Class.Exceptions.ConfigurationException;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly string? CONFIG_PATH = ConfigurationManager.AppSettings["configPath"]?.ToString();

        /// <summary>
        /// Method <c>GetAllStages</c> creates list of Stages based on a StageList object that returnes StageListBuilder.GetStageList method.
        /// </summary>
        /// <returns>Method returns list of Stages. If an error occures the list will be empty.</returns>
        /// <exception cref="ConfigurationException">Exception <c>ConfigurationException</c> is thrown if configuration path is not set.</exception>
        IList<Stage> IWorkflowRepository.GetAllStages()
        {
            IList<Stage> stages = new List<Stage>();

            try
            {
                if (CONFIG_PATH is null)
                {
                    throw new ConfigurationException("Missing configuration path.");
                }

                StageList? stagesList = StageListBuilder.GetStageList(CONFIG_PATH);
                if (stagesList != null && stagesList.Stages != null)
                {
                    stages = stagesList.Stages;
                }
            }
            catch (Exception e) when (e is ConfigurationException or WrongDefinitionException)
            {
                Console.WriteLine(e.Message);
            }

            return stages;
        }

    }
}
