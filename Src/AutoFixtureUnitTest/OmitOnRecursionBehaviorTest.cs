﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixtureUnitTest.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class OmitOnRecursionBehaviorTest
    {
        [Fact]
        public void SutIsSpecimenBuilderTransformation()
        {
            // Fixture setup
            // Exercise system
            var sut = new OmitOnRecursionBehavior();
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilderTransformation>(sut);
            // Teardown
        }

        [Fact]
        public void TransformNullBuilderThrows()
        {
            // Fixture setup
            var sut = new OmitOnRecursionBehavior();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Transform(null));
            // Teardown
        }

        [Fact]
        public void TransformReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new OmitOnRecursionBehavior();
            // Exercise system
            var dummyBuilder = new DelegatingSpecimenBuilder();
            var result = sut.Transform(dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<OmitOnRecursionGuard>(result);
            // Teardown
        }

        [Fact]
        public void TranformResultCorrectlyDecoratesInput()
        {
            // Fixture setup
            var sut = new OmitOnRecursionBehavior();
            var expectedBuilder = new DelegatingSpecimenBuilder();
            // Exercise system
            var result = sut.Transform(expectedBuilder);
            // Verify outcome
            var guard = Assert.IsAssignableFrom<OmitOnRecursionGuard>(result);
            Assert.Equal(expectedBuilder, guard.Builder);
            // Teardown
        }
    }
}
