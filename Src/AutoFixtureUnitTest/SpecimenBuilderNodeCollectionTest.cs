﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;
using Ploeh.AutoFixtureUnitTest.Kernel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class SpecimenBuilderNodeCollectionTest
    {
        private readonly ISpecimenBuilderNode graph;
        private readonly Func<ISpecimenBuilderNode, bool> adaptedCompositePredicate;
        private readonly SpecimenBuilderNodeCollection sut;

        public SpecimenBuilderNodeCollectionTest()
        {
            this.graph = new CompositeSpecimenBuilder(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder()),
                new MarkedNode(
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder()),
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder()));
            this.adaptedCompositePredicate = s => s is MarkedNode;
            this.sut = new SpecimenBuilderNodeCollection(this.graph, this.adaptedCompositePredicate);
        }

        [Fact]
        public void SutIsSpecimenBuilderList()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<IList<ISpecimenBuilder>>(this.sut);
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void IndexOfReturnsCorrectResult(int expected)
        {
            // Fixture setup
            var item = this.graph.OfType<MarkedNode>().Single().ElementAt(expected);
            // Exercise system
            var actual = this.sut.IndexOf(item);
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void IndexOfReturnsCorrectResultWhenItemIsNotInNode()
        {
            // Fixture setup
            var item = new DelegatingSpecimenBuilder();
            // Exercise system
            var actual = this.sut.IndexOf(item);
            // Verify outcome
            Assert.Equal(-1, actual);
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void InsertsCorrectlyInsertsItem(int expected)
        {
            // Fixture setup
            var item = new DelegatingSpecimenBuilder();
            // Exercise system
            this.sut.Insert(expected, item);
            // Verify outcome
            var actual = this.sut.IndexOf(item);
            Assert.Equal(expected, actual);
            // Teardown
        }

        private class MarkedNode : CompositeSpecimenBuilder
        {
            public MarkedNode(params ISpecimenBuilder[] builders)
                : base(builders)
            {
            }

            public MarkedNode(IEnumerable<ISpecimenBuilder> builders)
                : base(builders)
            {
            }

            public override ISpecimenBuilder Compose(IEnumerable<ISpecimenBuilder> builders)
            {
                return new MarkedNode(builders);
            }
        }
    }
}
