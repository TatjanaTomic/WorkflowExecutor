using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;
using CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml.StageListBuilder;
using ConfigurationException = CreatorMVVMProject.Model.Class.Exceptions.ConfigurationException;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly string? CONFIG_PATH = ConfigurationManager.AppSettings["configPath"]?.ToString();

        IList<Stage> IWorkflowRepository.GetAllStages()
        {
            IList<Stage> stages = new List<Stage>();

            try
            {
                if (CONFIG_PATH == null)
                    throw new ConfigurationException("Missing configuration path.");

                var stagesList = StageListBuilder.GetStageList(CONFIG_PATH);

                if (stagesList != null && stagesList.Stages != null)
                    stages = stagesList.Stages;

            } catch(ConfigurationException e)
            {
                Console.WriteLine(e.Message);
            }

            return stages;
        }

    }
}
