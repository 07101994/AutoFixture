﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Moq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class SealedPropertyInitializerTest
    {
        [Fact]
        public void SetupThrowsWhenMockIsNull()
        {
            // Fixture setup
            var context = new Mock<ISpecimenContext>();
            var sut = new SealedPropertyInitializer();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Setup(null, context.Object));
            // Teardown
        }

        [Fact]
        public void SetupThrowsWhenContextIsNull()
        {
            // Fixture setup
            var mock = new Mock<object>();
            var sut = new SealedPropertyInitializer();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Setup(mock, null));
            // Teardown
        }

        [Fact]
        public void InitializesSealedPropertyUsingContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithSealedMembers>();

            var sut = new SealedPropertyInitializer();
            // Exercise system
            sut.Setup(mock, new SpecimenContext(fixture));
            // Verify outcome
            Assert.Equal(frozenString, mock.Object.ExplicitlySealedProperty);
            Assert.Equal(frozenString, mock.Object.ImplicitlySealedProperty);
            // Teardown
        }

        [Fact]
        public void IgnoresGetOnlyProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var mock = new Mock<TypeWithGetOnlyProperty>();

            var sut = new SealedPropertyInitializer();
            //Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
        }

        [Fact]
        public void IgnoresVirtualProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithVirtualMembers>();

            var sut = new SealedPropertyInitializer();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, mock.Object.VirtualProperty);
        }

        [Fact]
        public void IgnoresPropertiesWithPrivateSetter()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithPropertyWithPrivateSetter>();

            var sut = new SealedPropertyInitializer();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, mock.Object.PropertyWithPrivateSetter);
        }

        [Fact]
        public void IgnoresPrivateProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithPrivateProperty>();
            var privateProperty = typeof (TypeWithPrivateProperty)
                .GetProperty("PrivateProperty",
                             BindingFlags.Instance | BindingFlags.NonPublic);

            var sut = new SealedPropertyInitializer();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, privateProperty.GetValue(mock.Object, null));
        }

        [Fact]
        public void IgnoresInterfaceProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithProperty>();

            var sut = new SealedPropertyInitializer();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, mock.Object.Property);
        }

        [Fact]
        public void IgnoresStaticProperties()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<StaticPropertyHolder<string>>();

            var sut = new SealedPropertyInitializer();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, StaticPropertyHolder<string>.Property);
        }
    }
}
