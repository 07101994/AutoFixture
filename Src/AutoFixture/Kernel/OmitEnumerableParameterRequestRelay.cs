﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class OmitEnumerableParameterRequestRelay : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var pi = (ParameterInfo)request;
            return context.Resolve(
                new SeededRequest(
                    pi.ParameterType,
                    pi.Name));
        }
    }
}
