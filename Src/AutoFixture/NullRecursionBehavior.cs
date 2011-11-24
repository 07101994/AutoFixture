﻿using System;
using Ploeh.AutoFixture.Kernel;
using System.Collections.Generic;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Decorates a <see cref="ISpecimenBuilder"/> with a <see cref="NullRecursionGuard"/>.
    /// </summary>
    public class NullRecursionBehavior : ISpecimenBuilderPipe, ISpecimenBuilderTransformation
    {
        /// <summary>
        /// Decorates the supplied <see cref="ISpecimenBuilder"/> with a
        /// <see cref="NullRecursionGuard"/>.
        /// </summary>
        /// <param name="builder">The builder to decorate.</param>
        /// <returns>
        /// <paramref name="builder"/> decorated with a <see cref="NullRecursionGuard"/>.
        /// </returns>
        public ISpecimenBuilder Transform(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            return new NullRecursionGuard(builder);
        }

        public IEnumerable<ISpecimenBuilder> Pipe(IEnumerable<ISpecimenBuilder> builders)
        {
            yield return new NullRecursionGuard(
                new CompositeSpecimenBuilder(
                    builders));
        }
    }
}
