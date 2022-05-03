using ExecutionEngine.Step;
using System.Xml.Serialization;
using StepStatus = ExecutionEngine.Step.StepStatus;

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
        public StepType Type { get; set; }

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

        public StepStatus Status { get; set; }


        public void Execute()
        {
            StepExecutor.StepExecutor.ExecuteStep(this);
        }
    }
}
