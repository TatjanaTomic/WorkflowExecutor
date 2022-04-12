﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEngine.Step
{
    public interface IParameter
    {
        string? KeyWord { get; }
        string? Value { get; }
    }
}