﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gAyPI.Manipulation
{
    public interface Injector
    {
        void Inject();

        object GetParams();
    }
}
