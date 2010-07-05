﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class ThrowingRecursionBehaviorTest
    {
        [Fact]
        public void SutIsSpecimenBuilderTransformation()
        {
            // Fixture setup
            // Exercise system
            var sut = new ThrowingRecursionBehavior();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderTransformation>(sut);
            // Teardown
        }

        [Fact]
        public void TransformNullBuilderThrows()
        {
            // Fixture setup
            var sut = new ThrowingRecursionBehavior();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Transform(null));
            // Teardown
        }

        [Fact]
        public void TransformReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new ThrowingRecursionBehavior();
            // Exercise system
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var result = sut.Transform(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ThrowingRecursionGuard>(result);
            // Teardown
        }

        [Fact]
        public void TranformResultCorrectlyDecoratesInput()
        {
            // Fixture setup
            var sut = new ThrowingRecursionBehavior();
            var expectedBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var result = sut.Transform(expectedBuilder);
            // Verify outcome
            var guard = Assert.IsAssignableFrom<ThrowingRecursionGuard>(result);
            Assert.Equal(expectedBuilder, guard.Builder);
            // Teardown
        }
    }
}
