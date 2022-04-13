﻿using ExecutionEngine.Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Stage
{
    public interface IStage<Step>
    {
        string Id { get; }
        List<Step>? Steps { get; }
    }
}
