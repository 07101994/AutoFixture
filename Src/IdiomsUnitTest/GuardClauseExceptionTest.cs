﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using System.Reflection;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class GuardClauseExceptionTest
    {
        [Fact]
        public void SutIsException()
        {
            // Fixture setup
            var dummyMember = typeof(object).GetMembers().First();
            var dummyValueType = typeof(object);
            // Exercise system
            var sut = new GuardClauseException(dummyMember, dummyValueType);
            // Verify outcome
            Assert.IsAssignableFrom<Exception>(sut);
            // Teardown
        }

        [Fact]
        public void MessageIsNotNull()
        {
            // Fixture setup
            var dummyMember = typeof(object).GetMembers().First();
            var dummyValueType = typeof(object);
            var sut = new GuardClauseException(dummyMember, dummyValueType);
            // Exercise system
            var result = sut.Message;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void MemberInfoIsCorrectWhenConstructedMinimally()
        {
            // Fixture setup
            var expectedMember = typeof(object).GetMembers().First();
            var dummyValueType = typeof(object);
            var sut = new GuardClauseException(expectedMember, dummyValueType);
            // Exercise system
            MemberInfo result = sut.MemberInfo;
            // Verify outcome
            Assert.Equal(expectedMember, result);
            // Teardown
        }

        [Fact]
        public void ValueTypeIsCorrectWhenConstructedMinimally()
        {
            // Fixture setup
            var dummyMember = typeof(object).GetMembers().First();
            var expectedValueType = typeof(object);
            var sut = new GuardClauseException(dummyMember, expectedValueType);
            // Exercise system
            Type result = sut.ValueType;
            // Verify outcome
            Assert.Equal(expectedValueType, result);
            // Teardown
        }
    }
}
