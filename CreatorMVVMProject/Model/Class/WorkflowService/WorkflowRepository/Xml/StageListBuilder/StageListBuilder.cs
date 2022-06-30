using CreatorMVVMProject.Model.Class.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml.StageListBuilder
{
    public static class StageListBuilder
    {
        public static StageList GetStageList(string configPath)
        {
            if (!File.Exists(configPath))
            {
                throw new ConfigurationException("Missing workflow configuration file.");
            }

            FileStream fileStream = File.Open(configPath, FileMode.Open);
            XmlSerializer serializer = new(typeof(StageList));

            var configuration = serializer.Deserialize(fileStream);
            if (configuration == null)
            {
                throw new ConfigurationException("Error deserializing XML configuration.");
            }

            var stageList = (StageList)configuration;
            if (stageList.Stages.Count != stageList.Stages.DistinctBy(stage => stage.Id).Count())
            {
                throw new ConfigurationException("Duplicate Stage ID entry.");
            }

            if (stageList.Stages.SelectMany(stage => stage.Steps).Count() != stageList.Stages.SelectMany(stage => stage.Steps).DistinctBy(step => step.Id).Count())
            {
                throw new ConfigurationException("Duplicate Step ID entry.");
            }

            return stageList;
        }
    }
}
