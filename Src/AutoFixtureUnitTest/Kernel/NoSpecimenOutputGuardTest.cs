﻿using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class NoSpecimenOutputGuardTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var sut = new NoSpecimenOutputGuard(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeModestCtorWithNullBuilderThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new NoSpecimenOutputGuard(null));
            // Teardown
        }

        [Fact]
        public void InitializeGreedyCtorWithNullBuilderThrows()
        {
            // Fixture setup
            var dummySpec = new DelegatingRequestSpecification();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new NoSpecimenOutputGuard(null, dummySpec));
            // Teardown
        }

        [Fact]
        public void InitializeGreedyCtorWithNullSpecificationThrows()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new NoSpecimenOutputGuard(dummyBuilder, null));
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrectWhenInitializedWithModestCtor()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var sut = new NoSpecimenOutputGuard(expectedBuilder);
            // Exercise system
            ISpecimenBuilder result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void BuilderIsCorrectWhenInitializedWithGreedyCtor()
        {
            // Fixture setup
            var expectedBuilder = new DelegatingSpecimenBuilder();
            var dummySpec = new DelegatingRequestSpecification();
            var sut = new NoSpecimenOutputGuard(expectedBuilder, dummySpec);
            // Exercise system
            var result = sut.Builder;
            // Verify outcome
            Assert.Equal(expectedBuilder, result);
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithModestCtor()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var sut = new NoSpecimenOutputGuard(dummyBuilder);
            // Exercise system
            IRequestSpecification result = sut.Specification;
            // Verify outcome
            Assert.IsAssignableFrom<TrueRequestSpecification>(result);
            // Teardown
        }

        [Fact]
        public void SpecificationIsCorrectWhenInitializedWithGreedyCtor()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var expectedSpec = new DelegatingRequestSpecification();
            var sut = new NoSpecimenOutputGuard(dummyBuilder, expectedSpec);
            // Exercise system
            var result = sut.Specification;
            // Verify outcome
            Assert.Equal(expectedSpec, result);
            // Teardown
        }

        [Fact]
        public void CreateReturnsCorrectResult()
        {
            // Fixture setup
            var request = new object();
            var context = new DelegatingSpecimenContext();
            var expectedResult = new object();
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => r == request && c == context ? expectedResult : new NoSpecimen(r) };

            var sut = new NoSpecimenOutputGuard(builder);
            // Exercise system
            var result = sut.Create(request, context);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void CreateThrowsWhenDecoratedBuilderReturnsNoSpecimen()
        {
            // Fixture setup
            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen(r) };
            var sut = new NoSpecimenOutputGuard(builder);
            // Exercise system and verify outcome
            var dummyRequest = new object();
            var dummyContext = new DelegatingSpecimenContext();
            Assert.Throws<ObjectCreationException>(() =>
                sut.Create(dummyRequest, dummyContext));
            // Teardown
        }

        [Fact]
        public void CreateDoesNotThrowOnReturnedNoSpecimenWhenSpecificationReturnsFalse()
        {
            // Fixture setup            
            var request = new object();

            var builder = new DelegatingSpecimenBuilder { OnCreate = (r, c) => new NoSpecimen(r) };
            var spec = new DelegatingRequestSpecification { OnIsSatisfiedBy = r => request == r ? false : true };
            var sut = new NoSpecimenOutputGuard(builder, spec);
            // Exercise system
            var dummyContext = new DelegatingSpecimenContext();
            var result = sut.Create(request, dummyContext);
            // Verify outcome
            var expectedResult = new NoSpecimen(request);
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
