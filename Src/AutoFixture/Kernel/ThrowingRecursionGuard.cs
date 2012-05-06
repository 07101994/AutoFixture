using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Handles recursion in the specimen creation process by throwing an exception at recursion point.
    /// </summary>
    public class ThrowingRecursionGuard : RecursionGuard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowingRecursionGuard"/> class.
        /// </summary>
        /// <param name="builder">The intercepting builder to decorate.</param>
        /// <param name="comparer">An IEqualitycomparer implementation to use when comparing requests to determine recursion.</param>
        public ThrowingRecursionGuard(ISpecimenBuilder builder, IEqualityComparer comparer)
            : base(builder, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowingRecursionGuard"/> class.
        /// </summary>
        /// <param name="builder">The builder.</param>
        public ThrowingRecursionGuard(ISpecimenBuilder builder)
            : base(builder)
        {
        }

        /// <summary>
        /// Handles a request that would cause recursion by throwing an exception.
        /// </summary>
        /// <param name="request">The recursion causing request.</param>
        /// <returns>Nothing. Always throws.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "AutoFixture", Justification = "Workaround for a bug in CA: https://connect.microsoft.com/VisualStudio/feedback/details/521030/")]
        public override object HandleRecursiveRequest(object request)
        {
            throw new ObjectCreationException(string.Format(
                CultureInfo.InvariantCulture,
                "AutoFixture was unable to create an instance of type {0} because the traversed object graph contains a circular reference. Information about the circular path follows below. This is the correct behavior when a Fixture is equipped with a ThrowingRecursionBehavior, which is the default. This ensures that you are being made aware of circular references in your code. Your first reaction should be to redesign your API in order to get rid of all circular references. However, if this is not possible (most likely because parts or all of the API is delivered by a third party), you can replace this default behaviour with a different behavior: on the Fixture instance, remove the ThrowingRecursionBehavior from Fixture.Behaviors, and instead add an instance of OmitOnRecursionBehavior.{2}\tPath:{2}{1}",
                this.RecordedRequests.Cast<object>().First().GetType(),
                this.GetFlattenedRequests(request),
                Environment.NewLine));
        }

        public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
        {
            var builder = CompositeSpecimenBuilder.ComposeIfMultiple(builders);
            return new ThrowingRecursionGuard(builder, this.Comparer);
        }

        private string GetFlattenedRequests(object finalRequest)
        {
            var requestInfos = new StringBuilder();
            foreach (object request in this.RecordedRequests)
            {
                Type type = request.GetType();
                if (type.Assembly != typeof(RecursionGuard).Assembly)
                {
                    requestInfos.Append("\t\t");
                    requestInfos.Append(request);
                    requestInfos.AppendLine(" --> ");
                }
            }

            requestInfos.Append("\t\t");
            requestInfos.AppendLine(finalRequest.ToString());

            return requestInfos.ToString();
        }
    }
}