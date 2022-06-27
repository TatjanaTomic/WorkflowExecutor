using System.Xml.Serialization;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml
{
    [XmlRoot("Dependency")]
    public class Dependency
    {
        public Dependency()
        {

        }

        [XmlAttribute("Id")]
        public string DependencyStepId { get; set; } = string.Empty;

    }
}
