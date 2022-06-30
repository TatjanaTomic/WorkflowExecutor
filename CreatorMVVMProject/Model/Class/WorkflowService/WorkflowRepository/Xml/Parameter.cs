using System.Xml.Serialization;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml
{
    [XmlRoot("Parameter")]
    public class Parameter
    {
        public Parameter()
        {

        }

        [XmlAttribute("KeyWord")]
        public string KeyWord { get; set; } = string.Empty;

        [XmlAttribute("Value")]
        public string Value { get; set; } = string.Empty;

        public override string? ToString()
        {
            return KeyWord + " " + Value + " ";
        }
    }
}
