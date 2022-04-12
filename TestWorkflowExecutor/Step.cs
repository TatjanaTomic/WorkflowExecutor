using ExecutionEngine.Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestExecutionEngine
{

    public class Dependency : IDependency
    {
        public string? DependencyStep { get; set; }
    }

    public class Parameter : IParameter
    {
        public string? KeyWord { get; set; }

        public string? Value { get; set; }
    }

    [XmlRoot("Step")]
    public class Step : IStepDetail<Dependency, Parameter>, IStep
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
