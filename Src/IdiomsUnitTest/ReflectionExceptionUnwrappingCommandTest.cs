﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using System.Reflection;
using Xunit.Extensions;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ReflectionExceptionUnwrappingCommandTest
    {
        [Fact]
        public void SutIsContextualCommand()
        {
            // Fixture setup
            var dummyCommand = new DelegatingGuardClauseCommand();
            // Exercise system
            var sut = new ReflectionExceptionUnwrappingCommand(dummyCommand);
            // Verify outcome
            Assert.IsAssignableFrom<IGuardClauseCommand>(sut);
            // Teardown
        }

        [Fact]
        public void CommandIsCorrect()
        {
            // Fixture setup
            var expectedCommand = new DelegatingGuardClauseCommand();
            var sut = new ReflectionExceptionUnwrappingCommand(expectedCommand);
            // Exercise system
            IGuardClauseCommand result = sut.Command;
            // Verify outcome
            Assert.Equal(expectedCommand, result);
            // Teardown
        }

        [Fact]
        public void ExecuteExecutesDecoratedCommand()
        {
            // Fixture setup
            var mockVerified = false;
            var expectedValue = new object();
            var cmd = new DelegatingGuardClauseCommand { OnExecute = v => mockVerified = expectedValue.Equals(v) };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Exercise system
            sut.Execute(expectedValue);
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }

        [Fact]
        public void ExecuteRethrowsNormalException()
        {
            // Fixture setup
            var cmd = new DelegatingGuardClauseCommand { OnExecute = v => { throw new InvalidOperationException(); } };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Exercise system and verify outcome
            var dummyValue = new object();
            Assert.Throws<InvalidOperationException>(() =>
                sut.Execute(dummyValue));
            // Teardown
        }

        [Fact]
        public void ExecuteUnwrapsAndThrowsInnerExceptionFromTargetInvocationException()
        {
            // Fixture setup
            var expectedException = new InvalidOperationException();
            var cmd = new DelegatingGuardClauseCommand { OnExecute = v => { throw new TargetInvocationException(expectedException); } };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Exercise system and verify outcome
            var dummyValue = new object();
            var e = Assert.Throws<InvalidOperationException>(() =>
                sut.Execute(dummyValue));
            Assert.Equal(expectedException, e);
            // Teardown
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(int))]
        [InlineData(typeof(string))]
        [InlineData(typeof(Version))]
        public void ContextTypeIsCorrect(Type type)
        {
            // Fixture setup
            var cmd = new DelegatingGuardClauseCommand { ContextType = type };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Exercise system
            var result = sut.ContextType;
            // Verify outcome
            Assert.Equal(type, result);
            // Teardown
        }

        [Fact]
        public void CreateExceptionReturnsCorrectResult()
        {
            // Fixture setup
            var value = Guid.NewGuid().ToString();
            var expected = new Exception();
            var cmd = new DelegatingGuardClauseCommand { OnCreateException = v => value == v ? expected : new Exception() };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Exercise system
            var result = sut.CreateException(value);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void CreateExceptionWithInnerReturnsCorrectResult()
        {
            // Fixture setup
            var value = Guid.NewGuid().ToString();
            var inner = new Exception();
            var expected = new Exception();
            var cmd = new DelegatingGuardClauseCommand { OnCreateExceptionWithInner = (v, e) => value == v && inner.Equals(e) ? expected : new Exception() };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Exercise system
            var result = sut.CreateException(value, inner);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }
    }
}
