using CreatorMVVMProject.Model.Class.Exceptions;
using System;
using System.IO;
using System.Xml.Serialization;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml.StageListBuilder
{
    public class StageListBuilder
    {
        private StageListBuilder() { }

        public static StageList GetStageList(string configPath)
        {
            if (!File.Exists(configPath))
            {
                throw new ConfigurationException("Missing workflow configuration file.");
            }

            try
            {
                FileStream fileStream = File.Open(configPath, FileMode.Open);
                XmlSerializer serializer = new(typeof(StageList));

                var configuration = serializer.Deserialize(fileStream);
                return configuration != null ? (StageList)configuration : throw new ConfigurationException("Error deserializing XML configuration.");

            }
            catch (Exception ex)
            {
                throw new ConfigurationException("Error reading XML configuration.", ex);
            }



        }
    }
}
