﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.AutoFixture.Idioms;
using Ploeh.TestTypeFoundation;
using System.Reflection;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class PropertySetCommandTest
    {
        [Fact]
        public void SutIsContextualAction()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<object>();
            var dummyProperty = dummyOwner.GetType().GetProperty("Property");
            // Exercise system
            var sut = new PropertySetCommand(dummyProperty, dummyOwner);
            // Verify outcome
            Assert.IsAssignableFrom<IContextualCommand>(sut);
            // Teardown
        }

        [Fact]
        public void PropertyInfoIsCorrect()
        {
            // Fixture setup
            var dummyOwner = new PropertyHolder<object>();
            var propertyInfo = dummyOwner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(propertyInfo, dummyOwner);
            // Exercise system
            PropertyInfo result = sut.PropertyInfo;
            // Verify outcome
            Assert.Equal(propertyInfo, result);
            // Teardown
        }

        [Fact]
        public void OwnerIsCorrect()
        {
            // Fixture setup
            var owner = new PropertyHolder<object>();
            var dummyProperty = owner.GetType().GetProperty("Property");
            var sut = new PropertySetCommand(dummyProperty, owner);
            // Exercise system
            var result = sut.Owner;
            // Verify outcome
            Assert.Equal(owner, result);
            // Teardown
        }
    }
}
