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

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void ContainsReturnsTrueForContainedItem(int index)
        {
            // Fixture setup
            var item = 
                this.graph.OfType<MarkedNode>().Single().ElementAt(index);
            // Exercise system
            var actual = this.sut.Contains(item);
            // Verify outcome
            Assert.True(actual);
            // Teardown
        }

        [Fact]
        public void ContainsReturnFalseForUncontainedItem()
        {
            // Fixture setup
            var uncontainedItem = new DelegatingSpecimenBuilder();
            // Exercise system
            var actual = this.sut.Contains(uncontainedItem);
            // Verify outcome
            Assert.False(actual);
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void RemoveAtCorrectlyRemovesItem(int index)
        {
            // Fixture setup
            var itemToBeRemoved =
                this.graph.OfType<MarkedNode>().Single().ElementAt(index);
            // Exercise system
            this.sut.RemoveAt(index);
            // Verify outcome
            Assert.False(this.sut.Contains(itemToBeRemoved));
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GetItemReturnsCorrectResult(int index)
        {
            // Fixture setup
            var expected = 
                this.graph.OfType<MarkedNode>().Single().ElementAt(index);
            // Exercise system
            var actual = this.sut[index];
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void GetItemForIncorrectIndexThrows()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                this.sut[1337]);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void SetItemCorrectlyAddesItem(int expected)
        {
            // Fixture setup
            var item = new DelegatingSpecimenBuilder();
            // Exercise system
            this.sut[expected] = item;
            // Verify outcome
            Assert.Equal(expected, this.sut.IndexOf(item));
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void SetItemCorrectlyRemovesExistingItem(int index)
        {
            // Fixture setup
            var itemToReplace = 
                this.graph.OfType<MarkedNode>().Single().ElementAt(index);
            // Exercise system
            this.sut[index] = new DelegatingSpecimenBuilder();
            // Verify outcome
            Assert.False(this.sut.Contains(itemToReplace));
            // Teardown
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(42)]
        [InlineData(3)]
        public void SetItemForIncorrectIndexThrows(int invalidIndex)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                this.sut[invalidIndex] = new DelegatingSpecimenBuilder());
        }

        [Fact]
        public void SutYieldsCorrectItems()
        {
            var expected = this.graph.OfType<MarkedNode>().Single();
            Assert.True(expected.SequenceEqual(this.sut));
            Assert.True(expected.Cast<object>().SequenceEqual(((System.Collections.IEnumerable)this.sut).Cast<object>()));
        }

        [Fact]
        public void CountReturnsCorrectResult()
        {
            // Fixture setup
            // Exercise system
            var actual = this.sut.Count;
            // Verify outcome
            var expected =
                this.graph.OfType<MarkedNode>().Single().Count();
            Assert.Equal(expected, actual);
            // Teardown
        }

        [Fact]
        public void ClearRemovesAllItems()
        {
            // Fixture setup
            // Exercise system
            this.sut.Clear();
            // Verify outcome
            Assert.Empty(this.sut);
            // Teardown
        }

        [Fact]
        public void AddAddsItemToEndOfNode()
        {
            // Fixture setup
            var item = new DelegatingSpecimenBuilder();
            var expected =
                this.graph.OfType<MarkedNode>().Single().Concat(new[] { item });
            // Exercise system
            this.sut.Add(item);
            // Verify outcome
            Assert.True(expected.SequenceEqual(this.sut));
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void CopyToCorrectlyCopiesItems(int index)
        {
            // Fixture setup
            var expected =
                this.graph.OfType<MarkedNode>().Single().ToArray();
            var a = new ISpecimenBuilder[expected.Length + index];
            // Exercise system
            this.sut.CopyTo(a, index);
            // Verify outcome
            Assert.True(expected.SequenceEqual(a.Skip(index)));
            // Teardown
        }

        [Fact]
        public void IsReadOnlyReturnsCorrectResult()
        {
            Assert.False(this.sut.IsReadOnly);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void RemoveCorrectlyRemovesItem(int index)
        {
            // Fixture setup
            var item =
                this.graph.OfType<MarkedNode>().Single().ElementAt(index);
            // Exercise system
            this.sut.Remove(item);
            // Verify outcome
            Assert.False(this.sut.Contains(item));
            // Teardown
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void RemoveReturnsTrueForContainedItem(int index)
        {
            // Fixture setup
            var item =
                this.graph.OfType<MarkedNode>().Single().ElementAt(index);
            // Exercise system
            var actual = this.sut.Remove(item);
            // Verify outcome
            Assert.True(actual);
            // Teardown
        }

        [Fact]
        public void RemoveReturnsFalseForUncontainedItem()
        {
            // Fixture setup
            var uncontainedItem = new DelegatingSpecimenBuilder();
            // Exercise system
            var actual = this.sut.Remove(uncontainedItem);
            // Verify outcome
            Assert.False(actual);
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

            public override ISpecimenBuilderNode Compose(IEnumerable<ISpecimenBuilder> builders)
            {
                return new MarkedNode(builders);
            }
        }
    }
}
