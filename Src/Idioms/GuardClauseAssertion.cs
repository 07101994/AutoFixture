﻿using System;
using System.Linq;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Encapsulates a unit test that verifies that a method or constructor has appropriate Guard
    /// Clauses in place.
    /// </summary>
    public class GuardClauseAssertion : IdiomaticAssertion
    {
        private readonly ISpecimenBuilder builder;
        private readonly IBehaviorExpectation behaviorExpectation;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardClauseAssertion"/> class.
        /// </summary>
        /// <param name="builder">
        /// A composer which can create instances required to implement the idiomatic unit test.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public GuardClauseAssertion(ISpecimenBuilder builder)
            : this(builder, new CompositeBehaviorExpectation(
                new NullReferenceBehaviorExpectation(),
                new EmptyGuidBehaviorExpectation()))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardClauseAssertion"/> class.
        /// </summary>
        /// <param name="builder">
        /// A composer which can create instances required to implement the idiomatic unit test.
        /// </param>
        /// <param name="behaviorExpectation">
        /// A behavior expectation to override the default expectation.
        /// </param>
        /// <remarks>
        /// <para>
        /// <paramref name="builder" /> will typically be a <see cref="Fixture" /> instance.
        /// </para>
        /// </remarks>
        public GuardClauseAssertion(ISpecimenBuilder builder, IBehaviorExpectation behaviorExpectation)
        {
            this.builder = builder;
            this.behaviorExpectation = behaviorExpectation;
        }

        /// <summary>
        /// Gets the builder supplied via the constructor.
        /// </summary>
        public ISpecimenBuilder Builder
        {
            get { return this.builder; }
        }

        /// <summary>
        /// Gets the behavior expectation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// GuardClauseAssertion contains an appropriate default implementation of
        /// <see cref="IBehaviorExpectation"/>, but a custom behavior can also be supplied via one
        /// of the constructor overloads. In any case, this property exposes the behavior
        /// expectation.
        /// </para>
        /// </remarks>
        /// <seealso cref="GuardClauseAssertion(ISpecimenBuilder, IBehaviorExpectation)" />
        public IBehaviorExpectation BehaviorExpectation
        {
            get { return this.behaviorExpectation; }
        }

        /// <summary>
        /// Verifies that a constructor has appripriate Guard Clauses in place.
        /// </summary>
        /// <param name="constructorInfo">The constructor.</param>
        /// <remarks>
        /// <para>
        /// Exactly which Guard Clauses are verified is defined by
        /// <see cref="BehaviorExpectation" />.
        /// </para>
        /// </remarks>
        public override void Verify(ConstructorInfo constructorInfo)
        {
            var method = new ConstructorMethod(constructorInfo);
            this.Verify(method, false);
        }

        /// <summary>
        /// Verifies that a method has appripriate Guard Clauses in place.
        /// </summary>
        /// <param name="methodInfo">The method.</param>
        /// <remarks>
        /// <para>
        /// Exactly which Guard Clauses are verified is defined by
        /// <see cref="BehaviorExpectation" />.
        /// </para>
        /// </remarks>
        public override void Verify(MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");

            if (methodInfo.IsEqualsMethod())
                return;

            var owner = this.Builder.CreateAnonymous(methodInfo.ReflectedType);
            var method = new InstanceMethod(methodInfo, owner);
            this.Verify(
                method,
                typeof(System.Collections.IEnumerable).IsAssignableFrom(methodInfo.ReturnType));
        }

        /// <summary>
        /// Verifies that a property has appripriate Guard Clauses in place.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        /// <remarks>
        /// <para>
        /// Exactly which Guard Clauses are verified is defined by
        /// <see cref="BehaviorExpectation" />.
        /// </para>
        /// </remarks>
        public override void Verify(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            if (propertyInfo.GetSetMethod() == null)
                return;

            var owner = this.Builder.CreateAnonymous(propertyInfo.ReflectedType);
            var command = new PropertySetCommand(propertyInfo, owner);
            var unwrapper = new ReflectionExceptionUnwrappingCommand(command);
            this.BehaviorExpectation.Verify(unwrapper);
        }

        private void Verify(IMethod method, bool isReturnValueIterator)
        {
            var parameters = (from pi in method.Parameters
                              select this.Builder.CreateAnonymous(GuardClauseAssertion.GetParameterType(pi))).ToList();

            var i = 0;
            foreach (var pi in method.Parameters)
            {
                var expansion = new IndexedReplacement<object>(i++, parameters);

                var command = new MethodInvokeCommand(method, expansion, pi);
                var unwrapper = new ReflectionExceptionUnwrappingCommand(command);
                if (isReturnValueIterator)
                {
                    var iteratorDecorator = new IteratorMethodInvokeCommand(unwrapper);
                    this.behaviorExpectation.Verify(iteratorDecorator);
                }
                else
                    this.BehaviorExpectation.Verify(unwrapper);
            }
        }

        private static Type GetParameterType(ParameterInfo pi)
        {
            var pType = pi.ParameterType;
            return pType.IsByRef ? pType.GetElementType() : pi.ParameterType;
        }

        private class IteratorMethodInvokeCommand : IGuardClauseCommand
        {
            private const string message = @"A Guard Clause test was performed on a method that may contain a deferred iterator block, but the test failed. See the inner exception for more details. However, because of the deferred nature of the iterator block, this test failure may look like a false positive. Perhaps you already have a Guard Clause in place, but in conjunction with the 'yield' keyword (if you're using C#); if this is the case, the Guard Clause is dormant, and will first be triggered when a client starts looping over the iterator. This doesn't adhere to the Fail Fast principle, so should be addressed.
See e.g. http://msmvps.com/blogs/jon_skeet/archive/2008/03/02/c-4-idea-iterator-blocks-and-parameter-checking.aspx for more details.";

            private readonly IGuardClauseCommand command;

            public IteratorMethodInvokeCommand(IGuardClauseCommand command)
            {
                this.command = command;
            }

            public Type RequestedType
            {
                get { return this.command.RequestedType; }
            }

            public void Execute(object value)
            {
                this.command.Execute(value);
            }

            public Exception CreateException(string value)
            {
                var e = this.command.CreateException(value);
                return new GuardClauseException(message, e);
            }

            public Exception CreateException(string value, Exception innerException)
            {
                var e = this.command.CreateException(value, innerException);
                return new GuardClauseException(message, e);
            }
        }

    }
}
