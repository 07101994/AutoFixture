﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using Moq;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// Post-processes a <see cref="Mock{T}"/> instance by setting appropriate default behavioral
    /// values.
    /// </summary>
    public class MockPostprocessor : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockPostprocessor"/> class with the
        /// supplied <see cref="ISpecimenBuilder"/>.
        /// </summary>
        /// <param name="builder">
        /// The builder which is expected to create <see cref="Mock{T}"/> instances.
        /// </param>
        public MockPostprocessor(ISpecimenBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            this.builder = builder;
        }

        /// <summary>
        /// Gets the builder decorated by this instance.
        /// </summary>
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Modifies a <see cref="Mock{T}"/> instance created by <see cref="Builder"/>.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The specimen created by <see cref="Builder"/>. If the instance is a correct
        /// <see cref="Mock{T}"/> instance, this instance modifies it before returning it.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            var t = request as Type;
            if (!t.IsMock())
            {
                return new NoSpecimen(request);
            }            

            var m = this.builder.Create(request, context) as Mock;
            if (m == null)
            {
                return new NoSpecimen(request);
            }

            var mockType = t.GetMockedType();
            if (m.GetType().GetMockedType() != mockType)
            {
                return new NoSpecimen(request);
            }

            var configurator = (IMockConfigurator)Activator.CreateInstance(typeof(MockConfigurator<>).MakeGenericType(mockType));
            configurator.Configure(m);

            return m;
        }

        #endregion

        private class MockConfigurator<T> : IMockConfigurator where T : class
        {
            #region IMockConfigurator Members

            public void Configure(Mock mock)
            {
                MockConfigurator<T>.Configure((Mock<T>)mock);
            }

            #endregion

            private static void Configure(Mock<T> mock)
            {
                mock.CallBase = true;
                mock.DefaultValue = DefaultValue.Mock;
            }
        }

        private interface IMockConfigurator
        {
            void Configure(Mock mock);
        }
    }
}
