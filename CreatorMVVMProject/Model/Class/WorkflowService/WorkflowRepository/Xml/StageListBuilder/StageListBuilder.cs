using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CreatorMVVMProject.Model.Class.Exceptions;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml.StageListBuilder
{
    public static class StageListBuilder
    {
        /// <summary>
        /// Method <c>GetStageList</c> deserializes the XML configuration file and checks if it is formatted correctly.
        /// </summary>
        /// <param name="configPath">Represents a path to XML configuration file.</param>
        /// <returns>Method returns </returns>
        /// <exception cref="ConfigurationException">Exception <c>ConfigurationException</c> is thrown if XML configuration file is missing or if it is formatted incorrectly.</exception>
        /// <exception cref="WrongDefinitionException">Exception <c>WrongDefinitionException</c> is thrown if exists duplicate Step ID or Stage ID.</exception>
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
                throw new WrongDefinitionException("Duplicate Stage ID entry.");
            }

            if (stageList.Stages.SelectMany(stage => stage.Steps).Count() != stageList.Stages.SelectMany(stage => stage.Steps).DistinctBy(step => step.Id).Count())
            {
                throw new WrongDefinitionException("Duplicate Step ID entry.");
            }

            return stageList;
        }
    }
}
