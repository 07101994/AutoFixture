﻿using System;
using System.Collections.Generic;
using AutoFixture.Kernel;
using TestTypeFoundation;
using Xunit;

namespace AutoFixtureUnitTest.Kernel
{
    public class SortedSetSpecificationTest
    {
        [Fact]
        public void SutIsRequestSpecification()
        {
            // Fixture setup
            // Exercise system
            var sut = new SortedSetSpecification();
            // Verify outcome
            Assert.IsAssignableFrom<IRequestSpecification>(sut);
            // Teardown
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(1)]
        [InlineData(typeof(object))]
        [InlineData(typeof(string))]
        [InlineData(typeof(int))]
        [InlineData(typeof(Version))]
        [InlineData(typeof(int?))]
        [InlineData(typeof(EmptyEnum?))]
        [InlineData(typeof(object[]))]
        [InlineData(typeof(string[]))]
        [InlineData(typeof(int[]))]
        [InlineData(typeof(Version[]))]
        [InlineData(typeof(int?[]))]
        [InlineData(typeof(EmptyEnum?[]))]
        public void IsSatisfiedByNonSortedSetRequestReturnsCorrectResult(object request)
        {
            // Fixture setup.
            var sut = new SortedSetSpecification();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(SortedSet<string>))]
        [InlineData(typeof(SortedSet<int>))]
        [InlineData(typeof(SortedSet<object>))]
        [InlineData(typeof(SortedSet<Version>))]
        [InlineData(typeof(SortedSet<int?>))]
        [InlineData(typeof(SortedSet<EmptyEnum?>))]
        [InlineData(typeof(SortedSet<string[]>))]
        [InlineData(typeof(SortedSet<int[]>))]
        [InlineData(typeof(SortedSet<object[]>))]
        [InlineData(typeof(SortedSet<Version[]>))]
        [InlineData(typeof(SortedSet<int?[]>))]
        [InlineData(typeof(SortedSet<EmptyEnum?[]>))]
        public void IsSatisfiedBySortedSetRequestReturnsCorrectResult(object request)
        {
            // Fixture setup
            var sut = new SortedSetSpecification();
            // Exercise system
            var result = sut.IsSatisfiedBy(request);
            // Verify outcome
            Assert.True(result);
            // Teardown
        }
    }
}