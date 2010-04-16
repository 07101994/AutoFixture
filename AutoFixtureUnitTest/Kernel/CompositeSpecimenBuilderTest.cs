﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class CompositeSpecimenBuilderTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeSpecimenBuilder();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void BuildersWillNotBeNullWhenSutIsCreatedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new CompositeSpecimenBuilder();
            // Exercise system
            IList<ISpecimenBuilder> result = sut.Builders;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void CreateWithNullEnumerableWillThrow()
        {
            // Fixture setup
            IEnumerable<ISpecimenBuilder> nullEnumerable = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeSpecimenBuilder(nullEnumerable));
            // Teardown
        }

        [Fact]
        public void BuildersWillMatchListParameter()
        {
            // Fixture setup
            var expectedBuilders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            }.AsEnumerable();
            var sut = new CompositeSpecimenBuilder(expectedBuilders);
            // Exercise system
            var result = sut.Builders;
            // Verify outcome
            Assert.True(expectedBuilders.SequenceEqual(result), "Builders");
            // Teardown
        }

        [Fact]
        public void CreateWithNullArrayWillThrow()
        {
            // Fixture setup
            ISpecimenBuilder[] nullArray = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CompositeSpecimenBuilder(nullArray));
            // Teardown
        }

        [Fact]
        public void BuildersWillMatchParamsArray()
        {
            // Fixture setup
            var expectedBuilders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder(),
                new DelegatingSpecimenBuilder()
            };
            var sut = new CompositeSpecimenBuilder(expectedBuilders[0], expectedBuilders[1], expectedBuilders[2]);
            // Exercise system
            var result = sut.Builders;
            // Verify outcome
            Assert.True(expectedBuilders.SequenceEqual(result), "Builders");
            // Teardown
        }

        [Fact]
        public void CreateWillReturnFirstSpecimenResultFromBuilders()
        {
            // Fixture setup
            var expectedResult = new object();
            var builders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedResult },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new object() }
            };
            var sut = new CompositeSpecimenBuilder(builders);
            // Exercise system
            var anonymousRequest = new object();
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(anonymousRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateWillReturnNoSpecimenfAllBuildersReturnNoSpecimen()
        {
            // Fixture setup
            var builders = new ISpecimenBuilder[]
            {
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() },
                new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen() }
            };
            var sut = new CompositeSpecimenBuilder(builders);
            // Exercise system
            var anonymousRequest = new object();
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(anonymousRequest, dummyContainer);
            // Verify outcome
            Assert.IsAssignableFrom<NoSpecimen>(result);
            // Teardown
        }

        [Fact]
        public void CreateWillInvokeBuilderWithCorrectRequest()
        {
            // Fixture setup
            var expectedRequest = new object();

            var mockVerified = false;
            var builderMock = new DelegatingSpecimenBuilder();
            builderMock.OnCreate = (r, c) =>
                {
                    Assert.Equal(expectedRequest, r);
                    mockVerified = true;
                    return new object();
                };

            var sut = new CompositeSpecimenBuilder(builderMock);
            // Exercise system
            var dummyContainer = new DelegatingSpecimenContainer();
            sut.Create(expectedRequest, dummyContainer);
            // Verify outcome
            Assert.True(mockVerified, "Mock verification");
            // Teardown
        }

        [Fact]
        public void CreateWillInvokeBuilderWithCorrectContainer()
        {
            // Fixture setup
            var expectedContainer = new DelegatingSpecimenContainer();

            var mockVerified = false;
            var builderMock = new DelegatingSpecimenBuilder();
            builderMock.OnCreate = (r, c) =>
                {
                    Assert.Equal(expectedContainer, c);
                    mockVerified = true;
                    return new object();
                };

            var sut = new CompositeSpecimenBuilder(builderMock);
            // Exercise system
            var dummyRequest = new object();
            sut.Create(dummyRequest, expectedContainer);
            // Verify outcome
            Assert.True(mockVerified, "Mock verification");
            // Teardown
        }
    }
}
