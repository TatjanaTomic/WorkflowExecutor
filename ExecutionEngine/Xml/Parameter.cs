using ExecutionEngine.Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExecutionEngine.Xml
{
    [XmlRoot("Parameter")]
    public class Parameter : IParameter
    {
        [XmlAttribute("KeyWord")]
        public string? KeyWord { get; set; }

        [XmlAttribute("Value")]
        public string? Value { get; set; }
    }
}
