using System.Xml.Serialization;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml
{
    [XmlRoot("Dependency")]
    public class Dependency
    {
        private string dependencyStepId = string.Empty;

        public Dependency()
        {

        }

        [XmlAttribute("Id")]
        public string DependencyStepId { get => this.dependencyStepId; set => this.dependencyStepId = value; }

    }
}
