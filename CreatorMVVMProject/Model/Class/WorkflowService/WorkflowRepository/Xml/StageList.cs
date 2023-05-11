using System.Collections.Generic;
using System.Xml.Serialization;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml;

[XmlRoot("Config")]
public class StageList
{
    public StageList()
    {

    }

    [XmlArray]
    [XmlArrayItem(ElementName = "Stage")]
    public List<Stage> Stages { get; set; } = new();

}
