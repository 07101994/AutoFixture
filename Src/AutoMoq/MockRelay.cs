﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;
using Moq;

namespace Ploeh.AutoFixture.AutoMoq
{
    /// <summary>
    /// Relays a request for an interface or an abstract class to a request for a
    /// <see cref="Mock{T}"/> of that class.
    /// </summary>
    public class MockRelay : ISpecimenBuilder
    {
        private readonly Func<Type, bool> shouldBeMocked;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockRelay"/> class.
        /// </summary>
        public MockRelay()
            : this(MockRelay.ShouldBeMocked)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockRelay"/> class with a specification
        /// that determines whether a type should be mocked.
        /// </summary>
        /// <param name="mockableSpecification">
        /// A specification that determines whether a type should be mocked or not.
        /// </param>
        public MockRelay(Func<Type, bool> mockableSpecification)
        {
            if (mockableSpecification == null)
            {
                throw new ArgumentNullException("mockableSpecification");
            }

            this.shouldBeMocked = mockableSpecification;
        }

        /// <summary>
        /// Gets a specification that determines whether a given type should be mocked.
        /// </summary>
        /// <value>The specification.</value>
        /// <remarks>
        /// <para>
        /// This specification determins whether a given type should be relayed as a request for a
        /// mock of the same type. By default it only returns <see langword="true"/> for interfaces
        /// and abstract classes, but a different specification can be supplied by using the
        /// <see cref="MockRelay(Func{Type, bool})"/> overloaded constructor that takes a
        /// specification as input. In that case, this property returns the specification supplied
        /// to the constructor.
        /// </para>
        /// </remarks>
        /// <seealso cref="MockRelay(Func{Type, bool})"/>
        public Func<Type, bool> MockableSpecification
        {
            get { return this.shouldBeMocked; }
        }

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A dynamic mock instance of the requested interface or abstract class if possible;
        /// otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var t = request as Type;
            if (!this.shouldBeMocked(t))
            {
                return new NoSpecimen(request);
            }

            var m = MockRelay.ResolveMock(t, context);
            if (m == null)
            {
                return new NoSpecimen(request);
            }

            return m.Object;
        }

        #endregion

        private static bool ShouldBeMocked(Type t)
        {
            return (t != null)
                && ((t.IsAbstract) || (t.IsInterface));
        }

        private static Mock ResolveMock(Type t, ISpecimenContext context)
        {
            var mockType = typeof(Mock<>).MakeGenericType(t);
            return context.Resolve(mockType) as Mock;
        }
    }
}
