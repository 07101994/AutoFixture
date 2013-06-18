﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class OmitOnRecursionHandlerTests
    {
        [Fact]
        public void SutIsRecursionHandler()
        {
            // Fixture setup
            // Exercise system
            var sut = new OmitOnRecursionHandler();
            // Verify outcome
            Assert.IsAssignableFrom<IRecursionHandler>(sut);
            // Teardown
        }

        [Fact]
        public void HandleRecursiveRequestReturnsCorrectResult()
        {
            // Fixture setup
            var sut = new OmitOnRecursionHandler();
            // Exercise system
            var dummyRequest = new object();
            var dummyRequests = Enumerable.Empty<object>();
            var actual = sut.HandleRecursiveRequest(dummyRequest, dummyRequests);
            // Verify outcome
            var expected = new OmitSpecimen();
            Assert.Equal(expected, actual);
            // Teardown
        }
    }
}
