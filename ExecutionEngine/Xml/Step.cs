using ExecutionEngine.Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExecutionEngine.Xml
{
    [XmlRoot("Step")]
    public class Step : IStep<Dependency, Parameter>
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("ExecutablePath")]
        public string? ExecutablePath { get; set; }

        [XmlAttribute("File")]
        public string? File { get; set; }

        [XmlAttribute("Type")]
        public TaskType Type { get; set; }

        [XmlAttribute("CanBeExecutedInParallel")]
        public bool CanBeExecutedInParallel { get; set; }

        [XmlElement]
        public string? Description { get; set; }

        [XmlArray]
        [XmlArrayItem(ElementName = "Dependency")]
        public List<Dependency>? Dependencies { get; set; }

        [XmlArray]
        [XmlArrayItem(ElementName = "Parameter")]
        public List<Parameter>? Parameters { get; set; }


        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
