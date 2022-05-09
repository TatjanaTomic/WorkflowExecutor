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
    public class Parameter
    {
        private string keyWord = string.Empty;
        private string value = string.Empty;

        public Parameter()
        {
                
        }

        [XmlAttribute("KeyWord")]
        public string KeyWord { get => keyWord; set => keyWord = value; }

        [XmlAttribute("Value")]
        public string Value { get => value; set => this.value = value; }

    }
}
