﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class ThrowingRecursionHandler : IRecursionHandler
    {
        public object HandleRecursiveRequest(
            object request,
            IEnumerable<object> recordedRequests)
        {
            throw new NotImplementedException();
        }
    }
}
