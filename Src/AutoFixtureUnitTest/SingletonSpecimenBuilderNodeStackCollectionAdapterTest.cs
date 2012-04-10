﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture;
using System.Collections.ObjectModel;
using Ploeh.AutoFixtureUnitTest.Kernel;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class SingletonSpecimenBuilderNodeStackCollectionAdapterTest
    {
        private readonly ISpecimenBuilderNode graph;
        private readonly SingletonSpecimenBuilderNodeStackCollectionAdapter sut;

        public SingletonSpecimenBuilderNodeStackCollectionAdapterTest()
        {
            this.graph = new MarkerNode(
                new CompositeSpecimenBuilder(
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder()),
                new CompositeSpecimenBuilder(new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder()),
                new CompositeSpecimenBuilder(new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder(),
                    new DelegatingSpecimenBuilder()));
            this.sut = new SingletonSpecimenBuilderNodeStackCollectionAdapter(this.graph);
        }

        [Fact]
        public void SutIsSpecimenBuilderTransformationList()
        {
            Assert.IsAssignableFrom<IList<ISpecimenBuilderTransformation>>(this.sut);
        }

        [Fact]
        public void SutIsCollection()
        {
            Assert.IsAssignableFrom<Collection<ISpecimenBuilderTransformation>>(this.sut);
        }

        [Fact]
        public void InitialGraphIsCorrect()
        {
            // Fixture setup
            // Exercise system
            ISpecimenBuilderNode actual = this.sut.Graph;
            // Verify outcome
            Assert.Equal(this.graph, actual);
            // Teardown
        }

        [Fact]
        public void InsertRaisesGraphChanged()
        {
            // Fixture setup
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Exercise system
            var dummyIndex = 0;
            var dummyItem = new DelegatingSpecimenBuilderTransformation();
            this.sut.Insert(dummyIndex, dummyItem);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void RemoveAtRaisesGraphChanged()
        {
            // Fixture setup
            this.sut.Add(new DelegatingSpecimenBuilderTransformation());
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Exercise system
            var dummyIndex = 0;
            this.sut.RemoveAt(dummyIndex);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void SetItemRaisesGraphChanged()
        {
            // Fixture setup
            this.sut.Add(new DelegatingSpecimenBuilderTransformation());
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Exercise system
            var dummyIndex = 0;
            var dummyItem = new DelegatingSpecimenBuilderTransformation();
            this.sut[dummyIndex] = dummyItem;
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void AddRaisesGraphChanged()
        {
            // Fixture setup
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Exercise system
            var dummyItem = new DelegatingSpecimenBuilderTransformation();
            this.sut.Add(dummyItem);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void ClearNonEmptyCollectionRaisesGraphChanged()
        {
            // Fixture setup
            this.sut.Add(new DelegatingSpecimenBuilderTransformation());
            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Exercise system
            this.sut.Clear();
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void ClearEmptyCollectionDoesNotRaiseGraphChanged()
        {
            // Fixture setup
            this.sut.Clear();
            var invoked = false;
            this.sut.GraphChanged += (s, e) => invoked = true;
            // Exercise system
            this.sut.Clear();
            // Verify outcome
            Assert.False(invoked);
            // Teardown
        }

        [Fact]
        public void RemoveContainedItemRaisesGraphChanged()
        {
            // Fixture setup
            var item = new DelegatingSpecimenBuilderTransformation();
            this.sut.Add(item);

            var verified = false;
            this.sut.GraphChanged += (s, e) => verified = s != null && e != null && e.Graph == this.sut.Graph;
            // Exercise system
            this.sut.Remove(item);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void RemoveUncontainedItemDoesNotRaiseGraphChanged()
        {
            // Fixture setup
            var item = new DelegatingSpecimenBuilderTransformation();
            var invoked = false;
            this.sut.GraphChanged += (s, e) => invoked = true;
            // Exercise system
            this.sut.Remove(item);
            // Verify outcome
            Assert.False(invoked);
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void InsertItemCorrectlyChangesGraph(int index)
        {
            // Fixture setup
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new CompositeSpecimenBuilder(b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new CompositeSpecimenBuilder(b) });
            this.sut.Add(new DelegatingSpecimenBuilderTransformation { OnTransform = b => new CompositeSpecimenBuilder(b) });            
            // Exercise system
            var item = new DelegatingSpecimenBuilderTransformation { OnTransform = b => new CompositeSpecimenBuilder(b) };
            this.sut.Insert(index, item);
            // Verify outcome
            var expected = this.sut.Aggregate(
                this.graph,
                (b, t) => (ISpecimenBuilderNode)t.Transform(b));

#warning Placeholder assertion waiting for a proper implementation of GraphEquals
            Assert.Equal(expected.GetType(), this.sut.Graph.GetType());
            // Teardown
        }

        private class MarkerNode : CompositeSpecimenBuilder
        {
            public MarkerNode(params ISpecimenBuilder[] builders)
                : base(builders)
            {
            }

            public MarkerNode(IEnumerable<ISpecimenBuilder> builders)
                : base(builders)
            {
            }

            public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
            {
                return new MarkerNode(builders);
            }
        }
    }
}
