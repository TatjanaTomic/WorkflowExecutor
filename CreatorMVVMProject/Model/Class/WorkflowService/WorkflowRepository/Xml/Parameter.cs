using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CreatorMVVMProject.Model.Class.WorkflowService.WorkflowRepository.Xml
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
        public string KeyWord { get => this.keyWord; set => this.keyWord = value; }

        [XmlAttribute("Value")]
        public string Value { get => this.value; set => this.value = value; }

        public override string? ToString()
        {
            return keyWord + " " + value + " ";
        }
    }
}
