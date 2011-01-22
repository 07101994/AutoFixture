﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit.Extensions;
using System.Collections.ObjectModel;

namespace Ploeh.AutoFixtureUnitTest
{
    public class MultipleCustomizationTest
    {
        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            // Exercise system
            var sut = new MultipleCustomization();
            // Verify outcome
            Assert.IsAssignableFrom<ICustomization>(sut);
            // Teardown
        }

        [Fact]
        public void CustomizeNullFixtureThrows()
        {
            // Fixture setup
            var sut = new MultipleCustomization();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.Customize(null));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(EnumerableRelay))]
        [InlineData(typeof(ListRelay))]
        [InlineData(typeof(CollectionRelay))]
        public void CustomizeAddsRelayToFixture(Type relayType)
        {
            // Fixture setup
            var sut = new MultipleCustomization();
            var fixture = new Fixture();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            Assert.True(fixture.ResidueCollectors.Any(b => relayType.IsAssignableFrom(b.GetType())));
            // Teardown
        }

        [Theory]
        [InlineData(typeof(ListSpecification), typeof(EnumerableFavoringConstructorQuery))]
        [InlineData(typeof(HashSetSpecification), typeof(EnumerableFavoringConstructorQuery))]
        [InlineData(typeof(CollectionSpecification), typeof(ListFavoringConstructorQuery))]
        public void CustomizeAddsBuilderForProperConcreteMultipleType(Type specificationType, Type queryType)
        {
            // Fixture setup
            var sut = new MultipleCustomization();
            var fixture = new Fixture();
            // Exercise system
            sut.Customize(fixture);
            // Verify outcome
            Assert.True(fixture.Customizations
                .OfType<FilteringSpecimenBuilder>()
                .Where(b => specificationType.IsAssignableFrom(b.Specification.GetType()))
                .Where(b => typeof(ConstructorInvoker).IsAssignableFrom(b.Builder.GetType()))
                .Select(b => (ConstructorInvoker)b.Builder)
                .Where(i => queryType.IsAssignableFrom(i.Query.GetType()))
                .Any());
            // Teardown
        }

        #region Usage/scenario tests

        [Fact]
        public void CreateAnonymousEnumerableReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<IEnumerable<Version>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousListReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<List<long>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousHashSetReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<HashSet<string>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousIListReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<IList<DateTime>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousICollectionReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<ICollection<TimeSpan>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        [Fact]
        public void CreateAnonymousCollectionReturnsCorrectResult()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new MultipleCustomization());
            // Exercise system
            var result = fixture.CreateAnonymous<Collection<Guid>>();
            // Verify outcome
            Assert.True(result.Any());
            // Teardown
        }

        #endregion
    }
}
