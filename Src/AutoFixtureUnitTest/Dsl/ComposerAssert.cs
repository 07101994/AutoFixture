﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Dsl
{
    internal static class ComposerAssert
    {
        internal static FilteringSpecimenBuilder IsFilter(this ISpecimenBuilder builder)
        {
            var filter = Assert.IsAssignableFrom<FilteringSpecimenBuilder>(builder);
            return filter;
        }

        internal static FilteringSpecimenBuilder ShouldContain(this FilteringSpecimenBuilder filter, ISpecimenBuilder expectedBuilder)
        {
            Assert.Equal(expectedBuilder, filter.Builder);
            return filter;
        }

        internal static FilteringSpecimenBuilder ShouldSpecify<T>(this FilteringSpecimenBuilder filter)
        {
            var spec = Assert.IsAssignableFrom<ExactTypeSpecification>(filter.Specification);
            Assert.Equal(typeof(T), spec.TargetType);

            return filter;
        }
    }
}
