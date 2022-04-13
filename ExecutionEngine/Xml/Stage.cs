using ExecutionEngine.Stage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExecutionEngine.Xml
{
    [XmlRoot("Stage")]
    public class Stage : IStage<Step>
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlArray]
        [XmlArrayItem(ElementName = "Step")]
        public List<Step>? Steps { get; set; }
    }
}
