﻿using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Creates anonymous variables from <see cref="ISpecimenContainer"/> or
    /// <see cref="ISpecimenBuilderComposer"/> instances.
    /// </summary>
    public static class SpecimenFactory
    {
        /// <summary>
        /// Creates an anonymous variable of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="container">The container used to resolve the type request.</param>
        /// <returns>An anonymous object of type <typeparamref name="T"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public static T CreateAnonymous<T>(this ISpecimenContainer container)
        {
            return (T)container.CreateAnonymous(default(T));
        }

        /// <summary>
        /// Creates an anonymous variable of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
        /// <returns>An anonymous object of type <typeparamref name="T"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public static T CreateAnonymous<T>(this ISpecimenBuilderComposer composer)
        {
            return composer.Compose().CreateContainer().CreateAnonymous<T>();
        }

        /// <summary>
        /// Creates an anonymous object, potentially using the supplied seed as additional
        /// information when creating the object.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="seed">
        /// Any data that adds additional information when creating the anonymous object.
        /// </param>
        /// <param name="container">The container used to resolve the type request.</param>
        /// <returns>An anonymous object.</returns>
        public static T CreateAnonymous<T>(this ISpecimenContainer container, T seed)
        {
            return (T)container.Resolve(new SeededRequest(typeof(T), seed));
        }

        /// <summary>
        /// Creates an anonymous object, potentially using the supplied seed as additional
        /// information when creating the object.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <param name="seed">
        /// Any data that adds additional information when creating the anonymous object.
        /// </param>
        /// <param name="composer">The composer used to resolve the type request.</param>
        /// <returns>An anonymous object.</returns>
        public static T CreateAnonymous<T>(this ISpecimenBuilderComposer composer, T seed)
        {
            return composer.Compose().CreateContainer().CreateAnonymous(seed);
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="container">The container used to resolve the type request.</param>
        /// <returns>A sequence of anonymous object of type <typeparamref name="T"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public static IEnumerable<T> CreateMany<T>(this ISpecimenContainer container)
        {
            return container.CreateMany(default(T));
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
        /// <returns>A sequence of anonymous object of type <typeparamref name="T"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public static IEnumerable<T> CreateMany<T>(this ISpecimenBuilderComposer composer)
        {
            return composer.Compose().CreateContainer().CreateMany<T>();
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="container">The container used to resolve the type request.</param>
        /// <param name="seed">
        /// An initial value that may or may not be used as input for the algorithm creating the
        /// return value.
        /// </param>
        /// <returns>A sequence of anonymous object of type <typeparamref name="T"/>.</returns>
        public static IEnumerable<T> CreateMany<T>(this ISpecimenContainer container, T seed)
        {
            return from s in (IEnumerable<object>)container.Resolve(new ManyRequest(new SeededRequest(typeof(T), seed)))
                   select (T)s;
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
        /// <param name="seed">
        /// An initial value that may or may not be used as input for the algorithm creating the
        /// return value.
        /// </param>
        /// <returns>A sequence of anonymous object of type <typeparamref name="T"/>.</returns>
        public static IEnumerable<T> CreateMany<T>(this ISpecimenBuilderComposer composer, T seed)
        {
            return composer.Compose().CreateContainer().CreateMany(seed);
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="container">The container used to resolve the type request.</param>
        /// <param name="count">The number of objects to create.</param>
        /// <returns>A sequence of anonymous objects of type <typeparamref name="T"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public static IEnumerable<T> CreateMany<T>(this ISpecimenContainer container, int count)
        {
            return container.CreateMany(default(T), count);
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
        /// <param name="count">The number of objects to create.</param>
        /// <returns>A sequence of anonymous objects of type <typeparamref name="T"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public static IEnumerable<T> CreateMany<T>(this ISpecimenBuilderComposer composer, int count)
        {
            return composer.Compose().CreateContainer().CreateMany<T>(count);
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="container">The container used to resolve the type request.</param>
        /// <param name="seed">
        /// An initial value that may or may not be used as input for the algorithm creating the
        /// return value.
        /// </param>
        /// <param name="count">The number of objects to create.</param>
        /// <returns>A sequence of anonymous objects of type <typeparamref name="T"/>.</returns>
        public static IEnumerable<T> CreateMany<T>(this ISpecimenContainer container, T seed, int count)
        {
            return from s in (IEnumerable<object>)container.Resolve(new FiniteSequenceRequest(new SeededRequest(typeof(T), seed), count))
                   select (T)s;
        }

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <param name="composer">The composer used to resolve the type request.</param>
        /// <param name="seed">
        /// An initial value that may or may not be used as input for the algorithm creating the
        /// return value.
        /// </param>
        /// <param name="count">The number of objects to create.</param>
        /// <returns>A sequence of anonymous objects of type <typeparamref name="T"/>.</returns>
        public static IEnumerable<T> CreateMany<T>(this ISpecimenBuilderComposer composer, T seed, int count)
        {
            return composer.Compose().CreateContainer().CreateMany(seed, count);
        }

        private static ISpecimenContainer CreateContainer(this ISpecimenBuilder builder)
        {
            return new DefaultSpecimenContainer(builder);
        }
    }
}
