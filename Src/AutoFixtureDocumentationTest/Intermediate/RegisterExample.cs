﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.AutoFixtureDocumentationTest.Intermediate
{
    public class RegisterExample
    {
        public RegisterExample()
        {
        }

        [Fact]
        public void CreatingMyClassIsNotPossibleWithoutRegister()
        {
            try
            {
                Fixture fixture = new Fixture();
                MyClass sut = fixture.CreateAnonymous<MyClass>();
            }
            catch (ObjectCreationException e)
            {
                Console.WriteLine(e);
            }
        }

        [Fact]
        public void SimpleRegister()
        {
            Fixture fixture = new Fixture();
            fixture.Register<IMyInterface>(() => 
                new FakeMyInterface());
            MyClass sut = fixture.CreateAnonymous<MyClass>();

            Assert.NotNull(sut);
        }

        [Fact]
        public void ManuallyRegisteringWithAnonymousParameter()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            int anonymousNumber = fixture.CreateAnonymous<int>();
            string knownText = "This text is not anonymous";
            fixture.Register<IMyInterface>(() => 
                new FakeMyInterface(anonymousNumber, knownText));
            // Exercise system
            MyClass sut = fixture.CreateAnonymous<MyClass>();
            // Verify outcome
            Assert.NotNull(sut);
            // Teardown
        }

        [Fact]
        public void RegisterWithAnonymousParameter()
        {
            // Fixture setup
            Fixture fixture = new Fixture();
            string knownText = "This text is not anonymous";
            fixture.Register<int, string, IMyInterface>((i, s) => 
                new FakeMyInterface(i, knownText));
            // Exercise system
            MyClass sut = fixture.CreateAnonymous<MyClass>();
            // Verify outcome
            Assert.NotNull(sut);
            // Teardown
        }
    }
}
