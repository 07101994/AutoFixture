﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class CompositeBehaviorExpectationTest
    {
        [Fact]
        public void SutIsBehaviorExpectation()
        {
            // Fixture setup
            // Exercise system
            var sut = new CompositeBehaviorExpectation();
            // Verify outcome
            Assert.IsAssignableFrom<IBehaviorExpectation>(sut);
            // Teardown
        }

        [Fact]
        public void ConstructedWithArrayBehaviorExpectationsIsCorrect()
        {
            // Fixture setup
            var expectations = new[] { new DelegatingBehaviorExpectation(), new DelegatingBehaviorExpectation(), new DelegatingBehaviorExpectation() };
            var sut = new CompositeBehaviorExpectation(expectations);
            // Exercise system
            IEnumerable<IBehaviorExpectation> result = sut.BehaviorExpectations;
            // Verify outcome
            Assert.True(expectations.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void ConstructedWithEnumerableBehaviorExpectationsIsCorrect()
        {
            // Fixture setup
            var expectations = new[] { new DelegatingBehaviorExpectation(), new DelegatingBehaviorExpectation(), new DelegatingBehaviorExpectation() }.Cast<IBehaviorExpectation>();
            var sut = new CompositeBehaviorExpectation(expectations);
            // Exercise system
            var result = sut.BehaviorExpectations;
            // Verify outcome
            Assert.True(expectations.SequenceEqual(result));
            // Teardown
        }
    }
}
