﻿using System;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    public class ObjectBuilderTest
    {
        [Fact]
        public void CreateAnonymousWillSetInt32Property()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<int>());
            // Exercise system
            PropertyHolder<int> result = sut.CreateAnonymous();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillSetInt32Field()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            var sut = ObjectBuilderTest.CreateSut(new FieldHolder<int>());
            // Exercise system
            FieldHolder<int> result = sut.CreateAnonymous();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Field);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillNotAttemptToSetReadOnlyProperty()
        {
            // Fixture setup
            int expectedNumber = default(int);
            var sut = ObjectBuilderTest.CreateSut(new ReadOnlyPropertyHolder<int>());
            // Exercise system
            ReadOnlyPropertyHolder<int> result = sut.CreateAnonymous();
            // Verify outcome
            Assert.Equal<int>(expectedNumber, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillNotAttemptToSetReadOnlyField()
        {
            // Fixture setup
            int expectedNumber = default(int);
            var sut = ObjectBuilderTest.CreateSut(new ReadOnlyFieldHolder<int>());
            // Exercise system
            ReadOnlyFieldHolder<int> result = sut.CreateAnonymous();
            // Verify outcome
            Assert.Equal<int>(expectedNumber, result.Field);
            // Teardown
        }

        [Fact]
        public void WithWillSetPropertyOnCreatedObject()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<string>());
            // Exercise system
            PropertyHolder<string> result = sut.With(ph => ph.Property, expectedText).CreateAnonymous();
            // Verify outcome
            Assert.Equal<string>(expectedText, result.Property);
            // Teardown
        }

        [Fact]
        public void WithWillSetFieldOnCreatedObject()
        {
            // Fixture setup
            string expectedText = "Anonymous text";
            var sut = ObjectBuilderTest.CreateSut(new FieldHolder<string>());
            // Exercise system
            FieldHolder<string> result = sut.With(fh => fh.Field, expectedText).CreateAnonymous();
            // Verify outcome
            Assert.Equal<string>(expectedText, result.Field);
            // Teardown
        }

        [Fact]
        public void AnonymousWithWillAssignPropertyEvenInCombinationWithOmitAutoProperties()
        {
            // Fixture setup
            long unexpectedNumber = default(long);
            var sut = ObjectBuilderTest.CreateSut(new DoublePropertyHolder<long, long>());
            // Exercise system
            var result = sut.With(ph => ph.Property1).OmitAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.NotEqual<long>(unexpectedNumber, result.Property1);
            // Teardown
        }

        [Fact]
        public void AnonymousWithWillAssignFieldEvenInCombinationWithOmitAutoProperties()
        {
            // Fixture setup
            int unexpectedNumber = default(int);
            var sut = ObjectBuilderTest.CreateSut(new DoubleFieldHolder<int, decimal>());
            // Exercise system
            var result = sut.With(fh => fh.Field1).OmitAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.NotEqual<int>(unexpectedNumber, result.Field1);
            // Teardown
        }

        [Fact]
        public void WithoutWillIgnorePropertyOnCreatedObject()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new DoublePropertyHolder<string, string>());
            // Exercise system
            var result = sut.Without(ph => ph.Property1).CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property1);
            // Teardown
        }

        [Fact]
        public void WithoutWillIgnorePropertyOnCreatedObjectEvenInCombinationWithWithAutoProperties()
        {
            // Fixture setup
            var fixture = new Fixture() { OmitAutoProperties = true };
            var sut = ObjectBuilderTest.CreateSut(fixture, new DoublePropertyHolder<string, string>());
            // Exercise system
            var result = sut.WithAutoProperties().Without(ph => ph.Property1).CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property1);
            // Teardown
        }

        [Fact]
        public void WithoutWillIgnoreFieldOnCreatedObject()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new DoubleFieldHolder<string, string>());
            // Exercise system
            var result = sut.Without(fh => fh.Field1).CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Field1);
            // Teardown
        }

        [Fact]
        public void WithoutWillNotIgnoreOtherPropertyOnCreatedObject()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new DoublePropertyHolder<string, string>());
            // Exercise system
            var result = sut.Without(ph => ph.Property1).CreateAnonymous();
            // Verify outcome
            Assert.NotNull(result.Property2);
            // Teardown
        }

        [Fact]
        public void WithoutWillNotIgnoreOtherFieldOnCreatedObject()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new DoubleFieldHolder<string, string>());
            // Exercise system
            var result = sut.Without(fh => fh.Field1).CreateAnonymous();
            // Verify outcome
            Assert.NotNull(result.Field2);
            // Teardown
        }

        [Fact]
        public void OmitAutoPropertiesWillNotAutoPopulateProperty()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<object>());
            // Exercise system
            PropertyHolder<object> result = sut.OmitAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.Null(result.Property);
            // Teardown
        }

        [Fact]
        public void WithAutoPropertiesWillAutoPopulateProperty()
        {
            // Fixture setup
            var fixture = new Fixture() { OmitAutoProperties = true };
            var sut = ObjectBuilderTest.CreateSut(fixture, new PropertyHolder<object>());
            // Exercise system
            PropertyHolder<object> result = sut.WithAutoProperties().CreateAnonymous();
            // Verify outcome
            Assert.NotNull(result.Property);
            // Teardown
        }

        [Fact]
        public void DoWillPerformOperationOnCreatedObject()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new CollectionHolder<object>());
            var expectedObject = new object();
            // Exercise system
            var result = sut.Do(x => x.Collection.Add(expectedObject)).CreateAnonymous().Collection.First();
            // Verify outcome
            Assert.Equal<object>(expectedObject, result);
            // Teardown
        }

        [Fact]
        public void BuilderSequenceWillBePreserved()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<int>());
            int expectedValue = 3;
            // Exercise system
            var result = sut.With(x => x.Property, 1)
                .Do(x => x.SetProperty(2))
                .With(x => x.Property, expectedValue)
                .CreateAnonymous();
            // Verify outcome
            Assert.Equal<int>(expectedValue, result.Property);
            // Teardown
        }

        [Fact]
        public void CreateAnonymousWillInvokeResolve()
        {
            // Fixture setup
            bool resolveWasInvoked = false;
            Func<Type, object> resolve = t =>
                {
                    resolveWasInvoked = true;
                    return new ConcreteType();
                };
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<AbstractType>(), resolve);
            // Exercise system
            sut.CreateAnonymous();
            // Verify outcome
            Assert.True(resolveWasInvoked, "Resolve");
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnUnregisteredAbstractionWillInvokeResolveCallbackWithCorrectType()
        {
            // Fixture setup
            Func<Type, object> resolve = t =>
                {
                    Assert.Equal<Type>(typeof(AbstractType), t);
                    return new ConcreteType();
                };
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<AbstractType>(), resolve);
            // Exercise system
            sut.CreateAnonymous();
            // Verify outcome (done by callback)
            // Teardown
        }

        [Fact]
        public void CreateAnonymousOnUnregisteredAbstractionWillReturnInstanceFromResolveCallback()
        {
            // Fixture setup
            var expectedValue = new ConcreteType();
            Func<Type, object> resolve = t => expectedValue;
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<AbstractType>(), resolve);
            // Exercise system
            var result = sut.CreateAnonymous().Property;
            // Verify outcome
            Assert.Equal<AbstractType>(expectedValue, result);
            // Teardown
        }

        [Fact]
        public void OmitAutoPropetiesWillNotMutateSut()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<string>());
            // Exercise system
            sut.OmitAutoProperties();
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.NotNull(instance.Property);
            // Teardown
        }

        [Fact]
        public void WithAutoPropetiesWillNotMutateSut()
        {
            // Fixture setup
            var fixture = new Fixture() { OmitAutoProperties = true };
            var sut = ObjectBuilderTest.CreateSut(fixture, new PropertyHolder<string>());
            // Exercise system
            sut.WithAutoProperties();
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.Null(instance.Property);
            // Teardown
        }

        [Fact]
        public void AnonymousWithWillNotMutateSut()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<string>()).OmitAutoProperties();
            // Exercise system
            sut.With(s => s.Property);
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.Null(instance.Property);
            // Teardown
        }

        [Fact]
        public void WithWillNotMutateSut()
        {
            // Fixture setup
            var unexpectedProperty = "Anonymous value";
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<string>());
            // Exercise system
            sut.With(s => s.Property, unexpectedProperty);
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.NotEqual(unexpectedProperty, instance.Property);
            // Teardown
        }

        [Fact]
        public void WithoutWillNotMutateSut()
        {
            // Fixture setup
            var sut = ObjectBuilderTest.CreateSut(new PropertyHolder<string>());
            // Exercise system
            sut.Without(s => s.Property);
            // Verify outcome
            var instance = sut.CreateAnonymous();
            Assert.NotNull(instance.Property);
            // Teardown
        }

        private static ObjectBuilder<T> CreateSut<T>(T obj)
        {
            Fixture f = new Fixture();
            return ObjectBuilderTest.CreateSut(f, obj);
        }

        private static ObjectBuilder<T> CreateSut<T>(T obj, Func<Type, object> resolve)
        {
            var f = new Fixture();
            f.Resolver = resolve;
            return f.Build<T>().WithConstructor(() => obj);
        }

        private static ObjectBuilder<T> CreateSut<T>(Fixture fixture, T obj)
        {
            return fixture.Build<T>().WithConstructor(() => obj);
        }
    }
}
