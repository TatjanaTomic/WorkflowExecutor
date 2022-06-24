using System.Collections.Generic;
using System.Xml.Serialization;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml
{
    [XmlRoot("Stage")]
    public class Stage
    {
        private string id = string.Empty;
        private List<Step> steps = new();

        public Stage()
        {

        }

        [XmlAttribute("Id")]
        public string Id { get => this.id; set => this.id = value; }

        [XmlArray]
        [XmlArrayItem(ElementName = "Step")]
        public List<Step> Steps { get => this.steps; set => this.steps = value; }

    }
}
