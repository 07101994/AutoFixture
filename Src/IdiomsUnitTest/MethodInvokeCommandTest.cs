﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using Xunit.Extensions;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class MethodInvokeCommandTest
    {
        [Fact]
        public void SutIsGuardClauseCommand()
        {
            // Fixture setup
            var dummyMethod = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion();
            // Exercise system
            var sut = new MethodInvokeCommand(dummyMethod, dummyExpansion);
            // Verify outcome
            Assert.IsAssignableFrom<IGuardClauseCommand>(sut);
            // Teardown
        }

        [Fact]
        public void MethodIsCorrect()
        {
            // Fixture setup
            var method = new DelegatingMethod();
            var dummyExpansion = new DelegatingExpansion();
            var sut = new MethodInvokeCommand(method, dummyExpansion);
            // Exercise system
            IMethod result = sut.Method;
            // Verify outcome
            Assert.Equal(method, result);
            // Teardown
        }

        [Fact]
        public void ExpansionIsCorrect()
        {
            // Fixture setup
            var dummyMethod = new DelegatingMethod();
            var expansion = new DelegatingExpansion();
            var sut = new MethodInvokeCommand(dummyMethod, expansion);
            // Exercise system
            IExpansion result = sut.Expansion;
            // Verify outcome
            Assert.Equal(expansion, result);
            // Teardown
        }
    }
}
