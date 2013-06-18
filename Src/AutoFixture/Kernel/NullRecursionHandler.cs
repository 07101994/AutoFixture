﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class NullRecursionHandler : IRecursionHandler
    {
        public object HandleRecursiveRequest(
            object request,
            IEnumerable<object> recordedRequests)
        {
            return null;
        }
    }
}
