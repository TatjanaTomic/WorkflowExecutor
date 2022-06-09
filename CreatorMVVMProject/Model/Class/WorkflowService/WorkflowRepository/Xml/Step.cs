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
        public string Id { get => this.id; set => this.id = value; }

        [XmlAttribute("ExecutablePath")]
        public string ExecutablePath { get => this.executablePath; set => this.executablePath = value; }

        [XmlAttribute("File")]
        public string File { get => this.file; set => this.file = value; }

        [XmlAttribute("Type")]
        public Type Type { get => this.type; set => this.type = value; }

        [XmlAttribute("CanBeExecutedInParallel")]
        public bool CanBeExecutedInParallel { get => this.canBeExecutedInParallel; set => this.canBeExecutedInParallel = value; }

        [XmlElement]
        public string Description { get => this.description; set => this.description = value; }

        [XmlArray]
        [XmlArrayItem(ElementName = "Dependency")]
        public List<Dependency> Dependencies { get => this.dependencies; set => this.dependencies = value; }

        [XmlArray]
        [XmlArrayItem(ElementName = "Parameter")]
        public List<Parameter> Parameters { get => this.parameters; set => this.parameters = value; }       

    }
}
