﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    internal class DelegatingTracingBuilder : TracingBuilder
    {
        public DelegatingTracingBuilder()
            : this(new DelegatingSpecimenBuilder())
        {
        }

        public DelegatingTracingBuilder(ISpecimenBuilder builder)
            : base(builder)
        {
        }

        internal void RaiseSpecimenCreated(SpecimenCreatedEventArgs e)
        {
            this.OnSpecimenCreated(e);
        }

        internal void RaiseSpecimenRequested(SpecimenTraceEventArgs e)
        {
            this.OnSpecimenRequested(e);
        }
    }
}
