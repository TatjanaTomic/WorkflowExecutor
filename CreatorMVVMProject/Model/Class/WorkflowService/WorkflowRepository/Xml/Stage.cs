using System.Collections.Generic;
using System.Xml.Serialization;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml
{
    [XmlRoot("Stage")]
    public class Stage
    {
        public Stage()
        {

        }

        [XmlAttribute("Id")]
        public string Id { get; set; } = string.Empty;

        [XmlArray]
        [XmlArrayItem(ElementName = "Step")]
        public List<Step> Steps { get; set; } = new();

    }
}
