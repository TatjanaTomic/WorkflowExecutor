﻿using ExecutionEngine.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExecutionEngine.Xml.StageListBuilder
{
    public class StageListBuilder
    {
        private StageListBuilder() { }

        public static StageList GetStageList(string configPath)
        {
            if (!File.Exists(configPath))
                throw new ConfigurationException("Missing configuration file.");

            try
            {
                FileStream fileStream = File.Open(configPath, FileMode.Open);
                XmlSerializer serializer = new(typeof(StageList));

                var configuration = serializer.Deserialize(fileStream);
                if (configuration == null)
                    throw new ConfigurationException("Error deserializing XML configuration.");
                else
                    return (StageList)configuration;

            }
            catch (Exception ex)
            {
                throw new ConfigurationException("Error reading XML configuration.", ex);
            }
            

            
        }
    }
}