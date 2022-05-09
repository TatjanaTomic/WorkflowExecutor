using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExecutionEngine.Xml
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
        public string Id { get => id; set => id=value; }

        [XmlArray]
        [XmlArrayItem(ElementName = "Step")]
        public List<Step> Steps { get => steps; set => steps = value; }

    }
}
