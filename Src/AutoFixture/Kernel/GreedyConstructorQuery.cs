﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Selects public constructors ordered by the most greedy constructor first.
    /// </summary>
#pragma warning disable 618
    public class GreedyConstructorQuery : IMethodQuery, IConstructorQuery
#pragma warning restore 618
    {
        #region IConstructorQuery Members

        /// <summary>
        /// Selects the constructors for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// All public constructors for <paramref name="type"/>, ordered by the most greedy
        /// constructor first.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The ordering of the returned constructors is based on the number of parameters of the
        /// constructor. Constructors with more parameters are returned before constructors with
        /// fewer parameters. This means that if a default constructor exists, it will be the last
        /// one returned.
        /// </para>
        /// <para>
        /// In case of two constructors with an equal number of parameters, the ordering is
        /// unspecified.
        /// </para>
        /// </remarks>
        public IEnumerable<IMethod> SelectConstructors(Type type)
        {
            return this.SelectMethods(type);
        }

        #endregion

        #region IMethodQuery Members

        /// <summary>
        /// Selects the constructors for the supplied type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// All public constructors for <paramref name="type"/>, ordered by the most greedy
        /// constructor first.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The ordering of the returned constructors is based on the number of parameters of the
        /// constructor. Constructors with more parameters are returned before constructors with
        /// fewer parameters. This means that if a default constructor exists, it will be the last
        /// one returned.
        /// </para>
        /// <para>
        /// In case of two constructors with an equal number of parameters, the ordering is
        /// unspecified.
        /// </para>
        /// </remarks>
        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return from ci in type.GetConstructors()
                   let parameters = ci.GetParameters()
                   orderby parameters.Length descending
                   select new ConstructorMethod(ci) as IMethod;
        }

        #endregion
    }
}
