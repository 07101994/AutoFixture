﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Relays a request for an <see cref="IDictionary{TKey, TValue}" /> to a request for a
    /// <see cref="Dictionary{TKey, TValue}"/> and retuns the result.
    /// </summary>
    public class DictionaryRelay : ISpecimenBuilder
    {
        /// <summary>
        /// Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// A populated <see cref="IDictionary{TKey, TValue}"/> of the appropriate item type if
        /// possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// If <paramref name="request"/> is a request for an
        /// <see cref="IDictionary{TKey, TValue}"/> and <paramref name="context"/> can satisfy a
        /// request for a populated specimen of that type, this value will be returned. If not, the
        /// return value is a <see cref="NoSpecimen"/> instance.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return (from t in request.Maybe().OfType<Type>()
                    let typeArguments = t.GetGenericArguments()
                    where typeArguments.Length == 2
                    && typeof(IDictionary<,>) == t.GetGenericTypeDefinition()
                    select context.Resolve(typeof(Dictionary<,>).MakeGenericType(typeArguments)))
                    .DefaultIfEmpty(new NoSpecimen(request))
                    .Single();
        }
    }
}
