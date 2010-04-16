﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Contact.ValueObject
{
    public class ContactTest
    {
        public ContactTest()
        {
        }

        [Fact]
        public void CreateWillNotThrow()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            // Exercise system
            fixture.CreateAnonymous<Contact>();
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void CreateWillPopulatePhoneNumber()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            Contact sut = fixture.CreateAnonymous<Contact>();
            // Exercise system
            int result = sut.PhoneNumber.RawNumber;
            // Verify outcome
            Assert.NotEqual<int>(default(int), result);
            // Teardown
        }
    }
}
