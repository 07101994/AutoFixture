﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    public interface ISpecimenBuilderNode : ISpecimenBuilder, IEnumerable<ISpecimenBuilder>
    {
        ISpecimenBuilder Compose(IEnumerable<ISpecimenBuilder> builders);
    }
}
