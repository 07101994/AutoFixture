﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest
{
    internal class LoopTest<TSut, TResult>
        where TSut : new()
    {
        private readonly Func<TSut, TResult> create;

        internal LoopTest(Func<TSut, TResult> func)
        {
            this.create = func;
        }

        public void Execute(int loopCount)
        {
            // Fixture setup
            TSut sut = new TSut();
            // Exercise system
            TResult result = default(TResult);
            for (int i = 0; i < loopCount; i++)
            {
                result = this.create(sut);
            }
            // Verify outcome
            Assert.Equal<TResult>((TResult)Convert.ChangeType(loopCount, typeof(TResult)), result);
            // Teardown
        }
    }
}
