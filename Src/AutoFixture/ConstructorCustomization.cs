﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// A customization that uses a particular constructor selection mechanism to pick and invoke
    /// a constructor to create specimens of the targeted type.
    /// </summary>
    public class ConstructorCustomization : ICustomization
    {
        private readonly Type targetType;
        private readonly IConstructorQuery query;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorCustomization"/> class.
        /// </summary>
        /// <param name="targetType">
        /// The <see cref="Type"/> for which <paramref name="query"/> should be used to select the
        /// most appropriate constructor.
        /// </param>
        /// <param name="query">
        /// The query that selects a constructor for <paramref name="targetType"/>.
        /// </param>
        public ConstructorCustomization(Type targetType, IConstructorQuery query)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            this.targetType = targetType;
            this.query = query;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> for which <see cref="Query"/> should be used to select the
        /// most appropriate constructor.
        /// </summary>
        public Type TargetType
        {
            get { return this.targetType; }
        }

        /// <summary>
        /// Gets the query that selects a constructor for <see cref="TargetType"/>.
        /// </summary>
        public IConstructorQuery Query
        {
            get { return this.query; }
        }

        #region ICustomization Members

        /// <summary>
        /// Customizes the specified fixture by modifying <see cref="TargetType"/> to use
        /// <see cref="Query"/> as the strategy for creating new specimens.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            var factory = new ConstructorInvoker(this.Query);
            var composer = new TypedBuilderComposer(this.TargetType, factory);

            fixture.Customizations.Insert(0, composer.Compose());
        }

        #endregion
    }
}
