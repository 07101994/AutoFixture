﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;
using System.Reflection;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class GuardClauseAssertionTest
    {
        [Fact]
        public void SutIsIdiomaticAssertion()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            // Exercise system
            var sut = new GuardClauseAssertion(dummyComposer);
            // Verify outcome
            Assert.IsAssignableFrom<IdiomaticAssertion>(sut);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrectFromModestConstructor()
        {
            // Fixture setup
            ISpecimenBuilderComposer expectedComposer = new Fixture();
            var sut = new GuardClauseAssertion(expectedComposer);
            // Exercise system
            var result = sut.Composer;
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void ComposerIsCorrectFromGreedyConstructor()
        {
            // Fixture setup
            ISpecimenBuilderComposer expectedComposer = new Fixture();
            var dummyExpectation = new DelegatingBehaviorExpectation();
            var sut = new GuardClauseAssertion(expectedComposer, dummyExpectation);
            // Exercise system
            ISpecimenBuilderComposer result = sut.Composer;
            // Verify outcome
            Assert.Equal(expectedComposer, result);
            // Teardown
        }

        [Fact]
        public void BehaviorExpectationIsCorrectFromGreedyConstructor()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            IBehaviorExpectation expected = new DelegatingBehaviorExpectation();
            var sut = new GuardClauseAssertion(dummyComposer, expected);
            // Exercise system
            IBehaviorExpectation result = sut.BehaviorExpectation;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void VerifyReadOnlyPropertyDoesNotThrow()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new GuardClauseAssertion(dummyComposer);
            // Exercise system and verify outcome
            var property = typeof(SingleParameterType<object>).GetProperty("Parameter");
            Assert.DoesNotThrow(() =>
                sut.Verify(property));
            // Teardown
        }

        [Fact]
        public void VerifyPropertyCorrectlyInvokesBehaviorExpectation()
        {
            // Fixture setup
            var fixture = new Fixture();
            var owner = fixture.Freeze<PropertyHolder<Version>>(c => c.OmitAutoProperties());
            var value = fixture.Freeze<Version>();

            var property = owner.GetType().GetProperty("Property");

            var mockVerified = false;
            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var setterCmd = Assert.IsAssignableFrom<PropertySetCommand>(unwrapper.Command);
                    mockVerified = setterCmd.PropertyInfo.Equals(property)
                        && setterCmd.Owner.Equals(owner);
                }
            };
            var sut = new GuardClauseAssertion(fixture, expectation);
            // Exercise system
            sut.Verify(property);
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }

        [Theory]
        [InlineData(typeof(GuardedMethodHost), 0)]
        [InlineData(typeof(GuardedMethodHost), 1)]
        [InlineData(typeof(GuardedMethodHost), 2)]
        [InlineData(typeof(GuardedMethodHost), 3)]
        [InlineData(typeof(GuardedMethodHost), 4)]
        [InlineData(typeof(GuardedMethodHost), 5)]
        [InlineData(typeof(GuardedMethodHost), 6)]
        [InlineData(typeof(GuardedMethodHost), 7)]
        [InlineData(typeof(GuardedMethodHost), 8)]
        [InlineData(typeof(Version), 0)]
        [InlineData(typeof(Version), 1)]
        [InlineData(typeof(Version), 2)]
        [InlineData(typeof(Version), 3)]
        [InlineData(typeof(Version), 4)]
        [InlineData(typeof(Version), 5)]
        [InlineData(typeof(Version), 6)]
        [InlineData(typeof(Version), 7)]
        [InlineData(typeof(Version), 8)]
        [InlineData(typeof(Version), 9)]
        [InlineData(typeof(Version), 10)]
        public void VerifyMethodInvokesBehaviorExpectationWithCorrectMethod(Type ownerType, int methodIndex)
        {
            // Fixture setup
            var method = ownerType.GetMethods().ElementAt(methodIndex);
            var parameters = method.GetParameters();

            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    var instanceMethod = Assert.IsAssignableFrom<InstanceMethod>(methodCmd.Method);
                    Assert.Equal(method, instanceMethod.Method);
                    Assert.IsAssignableFrom(ownerType, instanceMethod.Owner);
                    Assert.True(parameters.SequenceEqual(instanceMethod.Parameters));
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(method);
            // Verify outcome (done by mock)
            // Teardown
        }

        [Theory]
        [InlineData(typeof(GuardedMethodHost), 0)]
        [InlineData(typeof(GuardedMethodHost), 1)]
        [InlineData(typeof(GuardedMethodHost), 2)]
        [InlineData(typeof(GuardedMethodHost), 3)]
        [InlineData(typeof(GuardedMethodHost), 4)]
        [InlineData(typeof(GuardedMethodHost), 5)]
        [InlineData(typeof(GuardedMethodHost), 6)]
        [InlineData(typeof(GuardedMethodHost), 7)]
        [InlineData(typeof(GuardedMethodHost), 8)]
        [InlineData(typeof(Version), 0)]
        [InlineData(typeof(Version), 1)]
        [InlineData(typeof(Version), 2)]
        [InlineData(typeof(Version), 3)]
        [InlineData(typeof(Version), 4)]
        [InlineData(typeof(Version), 5)]
        [InlineData(typeof(Version), 6)]
        [InlineData(typeof(Version), 7)]
        [InlineData(typeof(Version), 8)]
        [InlineData(typeof(Version), 9)]
        [InlineData(typeof(Version), 10)]
        public void VerifyMethodInvokesBehaviorExpectationWithCorrectReplacementIndices(Type ownerType, int methodIndex)
        {
            // Fixture setup
            var method = ownerType.GetMethods().Where(IsNotEqualsMethod).ElementAt(methodIndex);
            var parameters = method.GetParameters();

            var observedIndices = new List<int>();
            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    var replacement = Assert.IsAssignableFrom<IndexedReplacement<object>>(methodCmd.Expansion);
                    observedIndices.Add(replacement.ReplacementIndex);
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(method);
            // Verify outcome
            var expectedIndices = Enumerable.Range(0, parameters.Length);
            Assert.True(expectedIndices.SequenceEqual(observedIndices));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(GuardedMethodHost), 0)]
        [InlineData(typeof(GuardedMethodHost), 1)]
        [InlineData(typeof(GuardedMethodHost), 2)]
        [InlineData(typeof(GuardedMethodHost), 3)]
        [InlineData(typeof(GuardedMethodHost), 4)]
        [InlineData(typeof(GuardedMethodHost), 5)]
        [InlineData(typeof(GuardedMethodHost), 6)]
        [InlineData(typeof(GuardedMethodHost), 7)]
        [InlineData(typeof(GuardedMethodHost), 8)]
        [InlineData(typeof(Version), 0)]
        [InlineData(typeof(Version), 1)]
        [InlineData(typeof(Version), 2)]
        [InlineData(typeof(Version), 3)]
        [InlineData(typeof(Version), 4)]
        [InlineData(typeof(Version), 5)]
        [InlineData(typeof(Version), 6)]
        [InlineData(typeof(Version), 7)]
        [InlineData(typeof(Version), 8)]
        [InlineData(typeof(Version), 9)]
        [InlineData(typeof(Version), 10)]
        public void VerifyMethodInvokesBehaviorExpectationWithCorrectParametersForReplacement(Type ownerType, int methodIndex)
        {
            // Fixture setup
            var method = ownerType.GetMethods().ElementAt(methodIndex);
            var parameters = method.GetParameters();

            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    var replacement = Assert.IsAssignableFrom<IndexedReplacement<object>>(methodCmd.Expansion);
                    Assert.True(replacement.Source.Select(x => x.GetType()).SequenceEqual(parameters.Select(p => p.ParameterType)));
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(method);
            // Verify outcome (done by mock)
            // Teardown
        }

        [Theory]
        [InlineData(typeof(GuardedMethodHost), 0)]
        [InlineData(typeof(GuardedMethodHost), 1)]
        [InlineData(typeof(GuardedMethodHost), 2)]
        [InlineData(typeof(GuardedMethodHost), 3)]
        [InlineData(typeof(GuardedMethodHost), 4)]
        [InlineData(typeof(GuardedMethodHost), 5)]
        [InlineData(typeof(GuardedMethodHost), 6)]
        [InlineData(typeof(GuardedMethodHost), 7)]
        [InlineData(typeof(GuardedMethodHost), 8)]
        [InlineData(typeof(Version), 0)]
        [InlineData(typeof(Version), 1)]
        [InlineData(typeof(Version), 2)]
        [InlineData(typeof(Version), 3)]
        [InlineData(typeof(Version), 4)]
        [InlineData(typeof(Version), 5)]
        [InlineData(typeof(Version), 6)]
        [InlineData(typeof(Version), 7)]
        [InlineData(typeof(Version), 8)]
        [InlineData(typeof(Version), 9)]
        [InlineData(typeof(Version), 10)]
        public void VerifyMethodInvokesBehaviorExpectationWithCorrectParameterInfo(Type ownerType, int methodIndex)
        {
            // Fixture setup
            var method = ownerType.GetMethods().Where(IsNotEqualsMethod).ElementAt(methodIndex);
            var parameters = method.GetParameters();

            var observedParameters = new List<ParameterInfo>();
            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    observedParameters.Add(methodCmd.ParameterInfo);
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(method);
            // Verify outcome
            Assert.True(parameters.SequenceEqual(observedParameters));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        public void VerifyMethodIgnoresEquals(Type type)
        {
            // Fixture setup
            var method = type.GetMethod("Equals", new[] { typeof(object) });

            var invoked = false;
            var expectation = new DelegatingBehaviorExpectation { OnVerify = c => invoked = true };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(method);
            // Verify outcome
            Assert.False(invoked);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(GuardedConstructorHost<object>), 0)]
        [InlineData(typeof(GuardedConstructorHost<string>), 0)]
        [InlineData(typeof(GuardedConstructorHost<Version>), 0)]
        [InlineData(typeof(ConcreteType), 0)]
        [InlineData(typeof(ConcreteType), 1)]
        [InlineData(typeof(ConcreteType), 2)]
        [InlineData(typeof(ConcreteType), 3)]
        [InlineData(typeof(ConcreteType), 4)]
        public void VerifyConstructorInvokesBehaviorExpectationWithCorrectMethod(Type type, int constructorIndex)
        {
            // Fixture setup
            var ctor = type.GetConstructors().ElementAt(constructorIndex);
            var parameters = ctor.GetParameters();

            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    var ctorMethod = Assert.IsAssignableFrom<ConstructorMethod>(methodCmd.Method);
                    Assert.Equal(ctor, ctorMethod.Constructor);
                    Assert.True(parameters.SequenceEqual(ctorMethod.Parameters));
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(ctor);
            // Verify outcome (done by mock)
            // Teardown
        }

        [Theory]
        [InlineData(typeof(GuardedConstructorHost<object>), 0)]
        [InlineData(typeof(GuardedConstructorHost<string>), 0)]
        [InlineData(typeof(GuardedConstructorHost<Version>), 0)]
        [InlineData(typeof(ConcreteType), 0)]
        [InlineData(typeof(ConcreteType), 1)]
        [InlineData(typeof(ConcreteType), 2)]
        [InlineData(typeof(ConcreteType), 3)]
        [InlineData(typeof(ConcreteType), 4)]
        public void VerifyConstructorInvokesBehaviorExpectationWithCorrectReplacementIndices(Type type, int constructorIndex)
        {
            // Fixture setup
            var ctor = type.GetConstructors().ElementAt(constructorIndex);
            var parameters = ctor.GetParameters();

            var observedIndices = new List<int>();
            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    var replacement = Assert.IsAssignableFrom<IndexedReplacement<object>>(methodCmd.Expansion);
                    observedIndices.Add(replacement.ReplacementIndex);
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(ctor);
            // Verify outcome
            var expectedIndices = Enumerable.Range(0, parameters.Length);
            Assert.True(expectedIndices.SequenceEqual(observedIndices));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(GuardedConstructorHost<object>), 0)]
        [InlineData(typeof(GuardedConstructorHost<string>), 0)]
        [InlineData(typeof(GuardedConstructorHost<Version>), 0)]
        [InlineData(typeof(ConcreteType), 0)]
        [InlineData(typeof(ConcreteType), 1)]
        [InlineData(typeof(ConcreteType), 2)]
        [InlineData(typeof(ConcreteType), 3)]
        [InlineData(typeof(ConcreteType), 4)]
        public void VerifyConstructorInvokesBehaviorExpectationWithCorrectParametersForReplacement(Type type, int constructorIndex)
        {
            // Fixture setup
            var ctor = type.GetConstructors().ElementAt(constructorIndex);
            var parameters = ctor.GetParameters();

            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    var replacement = Assert.IsAssignableFrom<IndexedReplacement<object>>(methodCmd.Expansion);
                    Assert.True(replacement.Source.Select(x => x.GetType()).SequenceEqual(parameters.Select(p => p.ParameterType)));
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(ctor);
            // Verify outcome (done by mock)
            // Teardown
        }

        [Theory]
        [InlineData(typeof(GuardedConstructorHost<object>), 0)]
        [InlineData(typeof(GuardedConstructorHost<string>), 0)]
        [InlineData(typeof(GuardedConstructorHost<Version>), 0)]
        [InlineData(typeof(ConcreteType), 0)]
        [InlineData(typeof(ConcreteType), 1)]
        [InlineData(typeof(ConcreteType), 2)]
        [InlineData(typeof(ConcreteType), 3)]
        [InlineData(typeof(ConcreteType), 4)]
        public void VerifyConstructorInvokesBehaviorExpectationWithCorrectParameterInfo(Type type, int constructorIndex)
        {
            // Fixture setup
            var ctor = type.GetConstructors().ElementAt(constructorIndex);
            var parameters = ctor.GetParameters();

            var observedParameters = new List<ParameterInfo>();
            var expectation = new DelegatingBehaviorExpectation
            {
                OnVerify = c =>
                {
                    var unwrapper = Assert.IsAssignableFrom<ReflectionExceptionUnwrappingCommand>(c);
                    var methodCmd = Assert.IsAssignableFrom<MethodInvokeCommand>(unwrapper.Command);

                    observedParameters.Add(methodCmd.ParameterInfo);
                }
            };

            var sut = new GuardClauseAssertion(new Fixture(), expectation);
            // Exercise system
            sut.Verify(ctor);
            // Verify outcome
            Assert.True(parameters.SequenceEqual(observedParameters));
            // Teardown
        }

        [Fact]
        public void DefaultBehaviorExpectationIsCorrect()
        {
            // Fixture setup
            var dummyComposer = new Fixture();
            var sut = new GuardClauseAssertion(dummyComposer);
            // Exercise system
            var result = sut.BehaviorExpectation;
            // Verify outcome
            var composite = Assert.IsAssignableFrom<CompositeBehaviorExpectation>(result);
            Assert.True(composite.BehaviorExpectations.OfType<NullReferenceBehaviorExpectation>().Any());
            // Teardown
        }

        private static bool IsNotEqualsMethod(MethodInfo method)
        {
            return !typeof(object).GetMethod("Equals", new[] { typeof(object) }).Equals(method.GetBaseDefinition());
        }
    }
}
