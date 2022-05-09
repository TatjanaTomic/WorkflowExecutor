using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExecutionEngine.Xml
{
    [XmlRoot("Config")]
    public class StageList
    {
        private List<Stage> stages = new();

        public StageList()
        {
           
        }

        [XmlArray]
        [XmlArrayItem(ElementName = "Stage")]
        public List<Stage> Stages { get => stages; set => stages = value; }

    }
}
