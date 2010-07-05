using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Base class for recursion handling. Tracks requests and reacts when a recursion point in the
    /// specimen creation process is detected.
    /// </summary>
    public abstract class RecursionGuard : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder builder;
        private readonly IEqualityComparer comparer;
        private readonly Stack<object> monitoredRequests;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard"/> class.
        /// </summary>
        /// <param name="builder">The intercepting builder to decorate.</param>
        protected RecursionGuard(ISpecimenBuilder builder)
            : this(builder, EqualityComparer<object>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursionGuard"/> class.
        /// </summary>
        /// <param name="builder">The intercepting builder to decorate.</param>
        /// <param name="comparer">
        /// An IEqualitycomparer implementation to use when comparing requests to determine recursion.
        /// </param>
        protected RecursionGuard(ISpecimenBuilder builder, IEqualityComparer comparer)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            this.monitoredRequests = new Stack<object>();
            this.builder = builder;
            this.comparer = comparer;
        }

        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        /// <summary>
        /// Gets the recorded requests so far.
        /// </summary>
        /// <value>The recorded requests.</value>
        protected object[] GetRecordedRequests()
        {
            return this.monitoredRequests.ToArray();
        }

        /// <summary>
        /// Handles a request that would cause recursion.
        /// </summary>
        /// <param name="request">The recursion causing request.</param>
        /// <returns>The specimen to return.</returns>
        public abstract object HandleRecursiveRequest(object request);

        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A container that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimen if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// 	<para>
        /// The <paramref name="request"/> can be any object, but will often be a
        /// <see cref="Type"/> or other <see cref="System.Reflection.MemberInfo"/> instances.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (this.monitoredRequests.Any(x => this.comparer.Equals(x, request)))
            {
                return this.HandleRecursiveRequest(request);
            }

            this.monitoredRequests.Push(request);
            var specimen = this.builder.Create(request, context);
            this.monitoredRequests.Pop();
            return specimen;
        }
    }
}