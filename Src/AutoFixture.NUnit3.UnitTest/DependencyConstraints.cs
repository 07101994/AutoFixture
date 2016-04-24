﻿using NUnit.Framework;
using System.Linq;

namespace Ploeh.AutoFixture.NUnit3.UnitTest
{
    public class DependencyConstraints
    {
        [InlineAutoData("FakeItEasy")]
        [InlineAutoData("Foq")]
        [InlineAutoData("FsCheck")]
        [InlineAutoData("Moq")]
        [InlineAutoData("NSubstitute")]
        [InlineAutoData("Rhino.Mocks")]
        [InlineAutoData("Unquote")]
        [InlineAutoData("xunit")]
        [InlineAutoData("xunit.extensions")]
        public void AutoFixtureNUnit3DoesNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = typeof(AutoDataAttribute).Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }

        [Theory]
        [InlineAutoData("FakeItEasy")]
        [InlineAutoData("Foq")]
        [InlineAutoData("FsCheck")]
        [InlineAutoData("Moq")]
        [InlineAutoData("NSubstitute")]
        [InlineAutoData("Rhino.Mocks")]
        [InlineAutoData("Unquote")]
        [InlineAutoData("xunit")]
        [InlineAutoData("xunit.extensions")]
        public void AutoFixtureNUnit3UnitTestsDoNotReference(string assemblyName)
        {
            // Fixture setup
            // Exercise system
            var references = this.GetType().Assembly.GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(an => an.Name == assemblyName));
            // Teardown
        }
    }
}
