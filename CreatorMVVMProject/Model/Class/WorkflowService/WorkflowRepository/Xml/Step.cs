using System.Collections.Generic;
using System.Xml.Serialization;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml
{
    [XmlRoot("Step")]
    public class Step
    {
        public Step()
        {

        }

        [XmlAttribute("Id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute("ExecutablePath")]
        public string ExecutablePath { get; set; } = string.Empty;

        [XmlAttribute("File")]
        public string File { get; set; } = string.Empty;

        [XmlAttribute("Type")]
        public Type Type { get; set; }

        [XmlAttribute("CanBeExecutedInParallel")]
        public bool CanBeExecutedInParallel { get; set; }

        [XmlElement]
        public string Description { get; set; } = string.Empty;

        [XmlArray]
        [XmlArrayItem(ElementName = "Dependency")]
        public List<Dependency> Dependencies { get; set; } = new();

        [XmlArray]
        [XmlArrayItem(ElementName = "Parameter")]
        public List<Parameter> Parameters { get; set; } = new();

    }
}
