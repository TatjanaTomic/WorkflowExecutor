﻿using ExecutionEngine.Stage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExecutionEngine.Xml
{
    [XmlRoot("Config")]
    public class StageList : IStageList<Stage>
    {
        [XmlArray]
        [XmlArrayItem(ElementName = "Stage")]
        public List<Stage>? Stages { get; set; }

    }
}
