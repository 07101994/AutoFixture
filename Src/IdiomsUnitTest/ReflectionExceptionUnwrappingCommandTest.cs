﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class ReflectionExceptionUnwrappingCommandTest
    {
        [Fact]
        public void SutIsContextualCommand()
        {
            // Fixture setup
            var dummyCommand = new DelegatingContextualCommand();
            // Exercise system
            var sut = new ReflectionExceptionUnwrappingCommand(dummyCommand);
            // Verify outcome
            Assert.IsAssignableFrom<IContextualCommand>(sut);
            // Teardown
        }

        [Fact]
        public void CommandIsCorrect()
        {
            // Fixture setup
            var expectedCommand = new DelegatingContextualCommand();
            var sut = new ReflectionExceptionUnwrappingCommand(expectedCommand);
            // Exercise system
            IContextualCommand result = sut.Command;
            // Verify outcome
            Assert.Equal(expectedCommand, result);
            // Teardown
        }

        [Fact]
        public void ExecuteExecutesDecoratedCommand()
        {
            // Fixture setup
            var mockVerified = false;
            var cmd = new DelegatingContextualCommand { OnExecute = () => mockVerified = true };
            var sut = new ReflectionExceptionUnwrappingCommand(cmd);
            // Exercise system
            sut.Execute();
            // Verify outcome
            Assert.True(mockVerified, "Mock verified.");
            // Teardown
        }
    }
}
