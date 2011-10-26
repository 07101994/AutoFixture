﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Dsl;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture
{
    /// <summary>
    /// Provides anonymous object creation services.
    /// </summary>
    public class Fixture : IFixture
    {
        private readonly List<ISpecimenBuilderTransformation> behaviors;
        private readonly CompositeSpecimenBuilder customizer;
        private readonly ISpecimenBuilder engine;
        private readonly CompositeSpecimenBuilder residueCollector;
        private readonly MultipleRelay multiple;

        /// <summary>
        /// Initializes a new instance of the <see cref="Fixture"/> class.
        /// </summary>
        public Fixture()
            : this(new DefaultEngineParts())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fixture"/> class with the supplied engine
        /// parts.
        /// </summary>
        /// <param name="engineParts">The engine parts.</param>
        public Fixture(DefaultRelays engineParts)
            : this(new CompositeSpecimenBuilder(engineParts), new MultipleRelay())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fixture"/> class with the supplied engine
        /// and a definition of what 'many' means.
        /// </summary>
        /// <param name="engine">The engine.</param>
        /// <param name="multiple">The definition and implementation of 'many'.</param>
        public Fixture(ISpecimenBuilder engine, MultipleRelay multiple)
        {
            if (engine == null)
            {
                throw new ArgumentNullException("engine");
            }
            if (multiple == null)
            {
                throw new ArgumentNullException("multiple");
            }

            this.customizer = new CompositeSpecimenBuilder();
            this.engine = engine;
            this.residueCollector = new CompositeSpecimenBuilder();

            this.Customizations.Add(new FilteringSpecimenBuilder(new MethodInvoker(new ModestConstructorQuery()), new NullableEnumRequestSpecification()));
            this.Customizations.Add(new EnumGenerator());

            this.multiple = multiple;

            this.behaviors = new List<ISpecimenBuilderTransformation>();
            this.behaviors.Add(new ThrowingRecursionBehavior());
        }

        /// <summary>
        /// Gets the behaviors that are applied when <see cref="Compose"/> is invoked.
        /// </summary>
        public IList<ISpecimenBuilderTransformation> Behaviors
        {
            get { return this.behaviors; }
        }

        /// <summary>
        /// Gets the customizations that intercept the <see cref="Engine"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Any <see cref="ISpecimenBuilder"/> in this list are invoked before
        /// <see cref="Engine"/>, giving them a chance to intercept a request and resolve it before
        /// the Engine.
        /// </para>
        /// <para>
        /// <see cref="Customize{T}"/> places resulting customizations in this list.
        /// </para>
        /// </remarks>
        /// <seealso cref="Engine"/>
        /// <seealso cref="ResidueCollectors"/>
        public IList<ISpecimenBuilder> Customizations
        {
            get { return this.customizer.Builders; }
        }

        /// <summary>
        /// Gets the core engine of the <see cref="Fixture"/> instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is the core engine that drives a <see cref="Fixture"/> instance. Even with no
        /// <see cref="Customizations"/> or <see cref="ResidueCollectors"/>, the
        /// <see cref="Engine"/> should be capably of resolving a wide range of different requests,
        /// based on conventions.
        /// </para>
        /// </remarks>
        /// <see cref="Customizations"/>
        /// <see cref="ResidueCollectors"/>
        public ISpecimenBuilder Engine
        {
            get { return this.engine; }
        }

        /// <summary>
        /// Gets or sets if writable properties should generally be assigned a value when 
        /// generating an anonymous object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default value is false.
        /// </para>
        /// </remarks>
        public bool OmitAutoProperties { get; set; }

        /// <summary>
        /// Gets or sets a number that controls how many objects are created when a
        /// <see cref="Fixture"/> creates more than one anonymous objects.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default value is 3.
        /// </para>
        /// </remarks>
        /// <seealso cref="CollectionFiller.AddManyTo{T}(IFixture, ICollection{T})" />
        /// <seealso cref="CollectionFiller.AddManyTo{T}(IFixture, ICollection{T}, Func{T})" />
        /// <seealso cref="SpecimenFactory.CreateMany{T}(ISpecimenBuilderComposer)" />
        /// <seealso cref="SpecimenFactory.CreateMany{T}(ISpecimenBuilderComposer, int)" />
        /// <seealso cref="Repeat"/>
        public int RepeatCount
        {
            get { return this.multiple.Count; }
            set { this.multiple.Count = value; }
        }

        /// <summary>
        /// Gets the residue collectors that can be used to handle requests that neither the
        /// <see cref="Customizations"/> nor <see cref="Engine"/> could handle.
        /// </summary>
        /// <remarks>
        /// <para>
        /// These <see cref="ISpecimenBuilder"/> instances will be invoked if no previous builder
        /// could resolve a request. This gives you the opportunity to define fallback strategies
        /// to deal with unresolved requests.
        /// </para>
        /// </remarks>
        public IList<ISpecimenBuilder> ResidueCollectors
        {
            get { return this.residueCollector.Builders; }
        }

        /// <summary>
        /// Customizes the creation algorithm for a single object.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object for which the algorithm should be customized.
        /// </typeparam>
        /// <returns>
        /// A <see cref="ICustomizationComposer{T}"/> that can be used to customize the creation
        /// algorithm before creating the object.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Although this CA warning should never be suppressed, this particular usage scenario has been discussed and accepted on the FxCop DL.")]
        public ICustomizationComposer<T> Build<T>()
        {
            return new CompositeComposer<T>(
                new BehaviorComposer<T>(
                    new Composer<T>().WithAutoProperties(this.EnableAutoProperties),
                    this.Behaviors),
                new NullComposer<T>(this.Compose));
        }

        /// <summary>
        /// Applies a customization.
        /// </summary>
        /// <param name="customization">The customization to apply.</param>
        /// <returns>
        /// The current instance.
        /// </returns>
        public IFixture Customize(ICustomization customization)
        {
            if (customization == null)
            {
                throw new ArgumentNullException("customization");
            }

            customization.Customize(this);
            return this;
        }

        /// <summary>
        /// Customizes the creation algorithm for all objects of a given type.
        /// </summary>
        /// <typeparam name="T">The type of object to customize.</typeparam>
        /// <param name="composerTransformation">
        /// A function that customizes a given <see cref="ICustomizationComposer{T}"/> and returns
        /// the modified composer.
        /// </param>
        /// <remarks>
        /// <para>
        /// The resulting <see cref="ISpecimenBuilder"/> is added to <see cref="Customizations"/>.
        /// </para>
        /// </remarks>
        public void Customize<T>(Func<ICustomizationComposer<T>, ISpecimenBuilderComposer> composerTransformation)
        {
            if (composerTransformation == null)
            {
                throw new ArgumentNullException("composerTransformation");
            }

            var c = composerTransformation(new Composer<T>().WithAutoProperties(this.EnableAutoProperties));
            this.customizer.Builders.Insert(0, c.Compose());
        }

        /// <summary>
        /// Repeats a function many times.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object that <paramref name="function"/> creates.
        /// </typeparam>
        /// <param name="function">
        /// A function that creates an instance of <typeparamref name="T"/>.
        /// </param>
        /// <returns>A sequence of objects created by <paramref name="function"/>.</returns>
        /// <remarks>
        /// <para>
        /// The number of times <paramref name="function"/> is invoked is determined by
        /// <see cref="RepeatCount"/>.
        /// </para>
        /// </remarks>
        public IEnumerable<T> Repeat<T>(Func<T> function)
        {
            return from f in Enumerable.Repeat(function, this.RepeatCount)
                   select f();
        }

        /// <summary>
        /// Composes a new <see cref="ISpecimenBuilder"/> instance that contains all the relevant
        /// strategies defined for this instance.
        /// </summary>
        /// <returns>
        /// A new <see cref="ISpecimenBuilder"/> instance that contains all the relevant strategies
        /// for the this <see cref="Fixture"/> instance, including <see cref="Customizations"/>,
        /// <see cref="Engine"/> and <see cref="ResidueCollectors"/>.
        /// </returns>
        public ISpecimenBuilder Compose()
        {
            ISpecimenBuilder builder = new CompositeSpecimenBuilder(
                this.Engine,
                this.multiple);

            if (this.EnableAutoProperties)
            {
                builder = new Postprocessor(
                    builder,
                    new AutoPropertiesCommand().Execute,
                    new AnyTypeSpecification());
            }

            builder = new CompositeSpecimenBuilder(
                this.customizer,
                builder,
                this.residueCollector,
                new TerminatingSpecimenBuilder());

            return this.Behaviors.Aggregate(
                builder,
                (b, behavior) =>
                    behavior.Transform(b));
        }

        private bool EnableAutoProperties
        {
            get { return !this.OmitAutoProperties; }
        }
    }
}
