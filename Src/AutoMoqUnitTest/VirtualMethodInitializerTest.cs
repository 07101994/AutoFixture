﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using Ploeh.AutoFixture.Kernel;
using Ploeh.TestTypeFoundation;
using Xunit;

namespace Ploeh.AutoFixture.AutoMoq.UnitTest
{
    public class VirtualMethodInitializerTest
    {
        [Fact]
        public void SetupThrowsWhenMockIsNull()
        {
            // Fixture setup
            var context = new Mock<ISpecimenContext>();
            var sut = new VirtualMethodInitializer();
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
            var sut = new VirtualMethodInitializer();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(
                () => sut.Setup(mock, null));
            // Teardown
        }

        [Fact]
        public void SetsUpInterfaceMethods_ToRetrieveReturnValueFromContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithParameterlessMethod>();

            var sut = new VirtualMethodInitializer();
            // Exercise system
            sut.Setup(mock, new SpecimenContext(fixture));
            // Verify outcome
            var result = mock.Object.Method();
            Assert.Same(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpVirtualMethods_ToRetrieveReturnValueFromContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithVirtualMembers>();

            var sut = new VirtualMethodInitializer();
            // Exercise system
            sut.Setup(mock, new SpecimenContext(fixture));
            // Verify outcome
            var result = mock.Object.VirtualMethod();
            Assert.Equal(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpPropertyGetters_ToRetrieveReturnValueFromContext()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithProperty>();

            var sut = new VirtualMethodInitializer();
            // Exercise system
            sut.Setup(mock, new SpecimenContext(fixture));
            // Verify outcome
            var result = mock.Object.Property;
            Assert.Equal(frozenString, result);
            // Teardown
        }

        [Fact]
        public void SetsUpMethodsWithParameters()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenObject = fixture.Freeze<object>();
            var mock = new Mock<IInterface>();

            var sut = new VirtualMethodInitializer();
            // Exercise system
            sut.Setup(mock, new SpecimenContext(fixture));
            // Verify outcome
            var result = mock.Object.MakeIt(4);
            Assert.Same(frozenObject, result);
            // Teardown
        }

        [Fact]
        public void SetsUpMethodsWithOutParameters()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenInt = fixture.Freeze<int>();
            var mock = new Mock<IInterfaceWithOutMethod>();

            var sut = new VirtualMethodInitializer();
            // Exercise system
            sut.Setup(mock, new SpecimenContext(fixture));
            // Verify outcome
            int outResult;
            mock.Object.Method(out outResult);
            Assert.Equal(frozenInt, outResult);
            // Teardown
        }

        [Fact]
        public void SetsUpIndexers()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenInt = fixture.Freeze<int>();
            var mock = new Mock<IInterfaceWithIndexer>();

            var sut = new VirtualMethodInitializer();
            // Exercise system
            sut.Setup(mock, new SpecimenContext(fixture));
            // Verify outcome
            int result = mock.Object[3];
            Assert.Equal(frozenInt, result);
            // Teardown
        }

        [Fact]
        public void SetsUpMethodsLazily()
        {
            // Fixture setup
            var context = new Mock<ISpecimenContext>();
            var mock = new Mock<IInterfaceWithParameterlessMethod>();

            var sut = new VirtualMethodInitializer();
            // Exercise system
            sut.Setup(mock, context.Object);
            // Verify outcome
            context.Verify(ctx => ctx.Resolve(It.IsAny<object>()), Times.Never());
            mock.Object.Method();
            context.Verify(ctx => ctx.Resolve(It.IsAny<object>()), Times.Once());
            // Teardown
        }

        [Fact]
        public void IgnoresMethodsWithRefParameters()
        {
            // Fixture setup
            var fixture = new Fixture();
            var mock = new Mock<IInterfaceWithRefMethod>();

            var sut = new VirtualMethodInitializer();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
        }

        [Fact]
        public void IgnoresSealedMethods()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<TypeWithSealedMembers>();

            var sut = new VirtualMethodInitializer();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, mock.Object.ImplicitlySealedMethod());
            Assert.NotEqual(frozenString, mock.Object.ExplicitlySealedMethod());
        }

        [Fact]
        public void IgnoresVoidMethods()
        {
            // Fixture setup
            var fixture = new Fixture();
            var mock = new Mock<IInterfaceWithVoidMethod>();

            var sut = new VirtualMethodInitializer();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
        }

        [Fact]
        public void IgnoresGenericMethods()
        {
            // Fixture setup
            var fixture = new Fixture();
            var frozenString = fixture.Freeze<string>();
            var mock = new Mock<IInterfaceWithGenericMethod>();

            var sut = new VirtualMethodInitializer();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
            Assert.NotEqual(frozenString, mock.Object.GenericMethod<string>());
        }

        [Fact]
        public void IgnoresStaticMethods()
        {
            // Fixture setup
            var fixture = new Fixture();
            var mock = new Mock<TypeWithStaticMethod>();

            var sut = new VirtualMethodInitializer();
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() => sut.Setup(mock, new SpecimenContext(fixture)));
        }
    }
}
