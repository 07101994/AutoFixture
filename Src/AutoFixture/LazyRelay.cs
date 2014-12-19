﻿using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Relays a request for an <see cref="Func{T}" /> to a request for a
    /// <see cref="Lazy{T}"/> and retuns the result.
    /// </summary>
    public class LazyRelay : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (request == null)
                return new NoSpecimen();

            var t = request as Type;
            if (t == null || !t.IsGenericType)
                return new NoSpecimen();

            if (t.GetGenericTypeDefinition() != typeof(Lazy<>))
                return new NoSpecimen();

            return typeof(LazyBuilder)
                 .GetMethod("Create")
                 .MakeGenericMethod(t.GetGenericArguments())
                 .Invoke(null, new[] { context });
        }

        private static class LazyBuilder
        {
            public static Lazy<T> Create<T>(ISpecimenContext context)
            {
                var f = (Func<T>)context.Resolve(typeof(Func<T>));
                return new Lazy<T>(f);
            }
        }
    }
}
