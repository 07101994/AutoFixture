﻿using System;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Decorates a <see cref="ISpecimenBuilder"/> with a <see cref="NullRecursionGuard"/>.
    /// </summary>
    public class NullRecursionBehavior : ISpecimenBuilderTransformation
    {
        #region ISpecimenBuilderTransformation Members

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

        #endregion
    }
}
