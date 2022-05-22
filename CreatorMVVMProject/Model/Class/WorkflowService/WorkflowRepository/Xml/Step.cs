using System.Collections.Generic;
using System.Xml.Serialization;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml
{
    [XmlRoot("Step")]
    public class Step
    {
        private string id = string.Empty;
        private string executablePath = string.Empty;
        private string file = string.Empty;
        private Type type;
        private bool canBeExecutedInParallel;
        private string description = string.Empty;
        private List<Dependency> dependencies = new();
        private List<Parameter> parameters = new();

        public Step()
        {

        }

        [XmlAttribute("Id")]
        public string Id { get => id; set => id = value; }

        [XmlAttribute("ExecutablePath")]
        public string ExecutablePath { get => executablePath; set => executablePath = value; }

        [XmlAttribute("File")]
        public string File { get => file; set => file = value; }

        [XmlAttribute("Type")]
        public Type Type { get => type; set => type = value; }

        [XmlAttribute("CanBeExecutedInParallel")]
        public bool CanBeExecutedInParallel { get => canBeExecutedInParallel; set => canBeExecutedInParallel = value; }

        [XmlElement]
        public string Description { get => description; set => description = value; }

        [XmlArray]
        [XmlArrayItem(ElementName = "Dependency")]
        public List<Dependency> Dependencies { get => dependencies; set => dependencies = value; }

        [XmlArray]
        [XmlArrayItem(ElementName = "Parameter")]
        public List<Parameter> Parameters { get => parameters; set => parameters = value; }       

    }
}
