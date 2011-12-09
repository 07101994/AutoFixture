﻿using System;
using Ploeh.AutoFixture.Kernel;
using System.Globalization;
using System.Linq;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// A customization that will freeze a specimen of a given <see cref="Type"/>.
    /// </summary>
    public class FreezingCustomization : ICustomization
    {
        private readonly Type targetType;
        private readonly Type registeredType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FreezingCustomization"/> class.
        /// </summary>
        /// <param name="targetType">The <see cref="Type"/> to freeze.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> is null.
        /// </exception>
        public FreezingCustomization(Type targetType)
            : this(targetType, targetType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FreezingCustomization"/> class.
        /// </summary>
        /// <param name="targetType">The <see cref="Type"/> to freeze.</param>
        /// <param name="registeredType">
        /// The <see cref="Type"/> to map the frozen <paramref name="targetType"/> value to.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="targetType"/> or <paramref name="registeredType"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="registeredType"/> is not assignable from <paramref name="targetType"/>.
        /// </exception>
        public FreezingCustomization(Type targetType, Type registeredType)
        {
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            if (registeredType == null)
            {
                throw new ArgumentNullException("registeredType");
            }

            if (!registeredType.IsAssignableFrom(targetType))
            {
                var message = String.Format(
                    CultureInfo.CurrentCulture,
                    "The type '{0}' cannot be frozen as '{1}' because the two types are not compatible.",
                    targetType,
                    registeredType);
                throw new ArgumentException(message);
            }

            this.targetType = targetType;
            this.registeredType = registeredType;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> to freeze.
        /// </summary>
        public Type TargetType
        {
            get { return targetType; }
        }

        /// <summary>
        /// Gets the <see cref="Type"/> to which the frozen <see cref="TargetType"/> value
        /// should be mapped to. Defaults to the same <see cref="Type"/> as <see cref="TargetType"/>.
        /// </summary>
        public Type RegisteredType
        {
            get { return registeredType; }
        }

        /// <summary>
        /// Customizes the fixture by freezing the value of <see cref="TargetType"/>.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fixture"/> is null.
        /// </exception>
        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException("fixture");
            }

            new CustomizeCommand(fixture, this).Execute();
        }

        private class CustomizeCommand
        {
            private readonly IFixture fixture;
            private readonly FreezingCustomization customization;

            internal CustomizeCommand(
                IFixture fixture,
                FreezingCustomization customization)
            {
                this.fixture = fixture;
                this.customization = customization;
            }

            internal void Execute()
            {
                var fixedBuilder =
                    CreateFixedSpecimenBuilderForTargetType();
                var builder = new FixedBuilderComposer(
                    fixedBuilder,
                    this.customization.targetType,
                    this.customization.registeredType).Compose();
                this.fixture.Customizations.Insert(
                    0, builder);
            }

            private FixedBuilder CreateFixedSpecimenBuilderForTargetType()
            {
                var specimen =
                    this.CreateSpecimenForTargetType();
                return new FixedBuilder(specimen);
            }

            private object CreateSpecimenForTargetType()
            {
                var context = new SpecimenContext(this.fixture.Compose());
                return context.Resolve(this.customization.targetType);
            }

            private class FixedBuilderComposer : ISpecimenBuilderComposer
            {
                private readonly FixedBuilder fixedBuilder;
                private readonly Type[] types;

                internal FixedBuilderComposer(
                    FixedBuilder fixedBuilder,
                    params Type[] types)
                {
                    this.fixedBuilder = fixedBuilder;
                    this.types = types;
                }

                public ISpecimenBuilder Compose()
                {
                    return new CompositeSpecimenBuilder(
                        from t in this.types
                        select this.MapFixedSpecimenBuilderTo(t));
                }

                private ISpecimenBuilder MapFixedSpecimenBuilderTo(Type type)
                {
                    var builderComposer =
                        new TypedBuilderComposer(
                            type, this.fixedBuilder);
                    return builderComposer.Compose();
                }
            }
        }
    }
}
