using ExecutionEngine.Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExecutionEngine.Xml
{
    [XmlRoot("Dependency")]
    public class Dependency
    {
        private string dependencyStepId = string.Empty;

        public Dependency()
        {

        }

        [XmlAttribute("Id")]
        public string DependencyStepId { get => dependencyStepId; set => dependencyStepId = value; }

    }
}
