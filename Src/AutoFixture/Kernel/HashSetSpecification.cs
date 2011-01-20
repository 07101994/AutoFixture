﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public class HashSetSpecification : IRequestSpecification
    {
        #region IRequestSpecification Members

        public bool IsSatisfiedBy(object request)
        {
            var type = request as Type;
            if (type == null)
            {
                return false;
            }

            return type.IsGenericType
                && typeof(HashSet<>) == type.GetGenericTypeDefinition();
        }

        #endregion
    }
}
