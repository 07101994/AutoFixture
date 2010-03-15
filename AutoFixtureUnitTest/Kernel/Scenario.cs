﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture;
using Ploeh.TestTypeFoundation;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    [TestClass]
    public class Scenario
    {
        [TestMethod]
        public void CreateSingleStringParameterizedType()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = (SingleParameterType<string>)container.Create(typeof(SingleParameterType<string>));
            // Verify outcome
            var name = new TextGuidRegex().GetText(result.Parameter);
            string guidString = new TextGuidRegex().GetGuid(result.Parameter);
            Guid g = new Guid(guidString);
            Assert.AreEqual("parameter", name, "Create");
            Assert.AreNotEqual<Guid>(Guid.Empty, g, "Guid part");
            // Teardown
        }

        [TestMethod]
        public void CreateDoubleStringParameterizedType()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = (DoubleParameterType<string, string>)container.Create(typeof(DoubleParameterType<string, string>));
            // Verify outcome
            Assert.IsFalse(string.IsNullOrEmpty(result.Parameter1), "Parameter1");
            Assert.IsFalse(string.IsNullOrEmpty(result.Parameter2), "Parameter2");
            // Teardown
        }

        [TestMethod]
        public void CreateStringAndIntegerParameterizedType()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = (DoubleParameterType<string, int>)container.Create(typeof(DoubleParameterType<string, int>));
            // Verify outcome
            Assert.IsFalse(string.IsNullOrEmpty(result.Parameter1), "Parameter11");
            Assert.AreNotEqual(0, result.Parameter2, "Parameter2");
            // Teardown
        }

        [TestMethod]
        public void CreateDecimalAndBooleanParameterizedType()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = (DoubleParameterType<decimal, bool>)container.Create(typeof(DoubleParameterType<decimal, bool>));
            // Verify outcome
            Assert.AreEqual(1m, result.Parameter1, "Parameter1");
            Assert.IsTrue(result.Parameter2, "Parameter2");
            // Teardown
        }

        [TestMethod]
        public void CreateNestedType()
        {
            // Fixture setup
            var container = Scenario.CreateContainer();
            // Exercise system
            var result = (DoubleParameterType<DoubleParameterType<int, Guid>, DoubleParameterType<decimal, bool>>)container.Create(
                typeof(DoubleParameterType<DoubleParameterType<int, Guid>, DoubleParameterType<decimal, bool>>));
            // Verify outcome
            Assert.AreEqual(1, result.Parameter1.Parameter1, "Parameter1.Parameter1");
            Assert.AreNotEqual(default(Guid), result.Parameter1.Parameter2, "Parameter1.Parameter2");
            Assert.AreEqual(1m, result.Parameter2.Parameter1, "Parameter2.Parameter1");
            Assert.AreEqual(true, result.Parameter2.Parameter2, "Parameter2.Parameter2");
            // Teardown
        }

        [TestMethod]
        public void CreateDoubleMixedParameterizedTypeWithNumberBasedStringGenerator()
        {
            // Fixture setup
            var intGenerator = new Int32SequenceGenerator();
            var builder = new CompositeSpecimenBuilder(
                intGenerator,
                new StringGenerator(() => intGenerator.CreateAnonymous()),
                new DecimalSequenceGenerator(),
                new BooleanSwitch(),
                new GuidGenerator(),
                new ModestConstructorInvoker(),
                new ParameterRequestTranslator(),
                new StringSeedUnwrapper(),
                new ValueIgnoringSeedUnwrapper());
            var container = new DefaultSpecimenContainer(builder);
            // Exercise system
            var result = (TripleParameterType<int, string, int>)container.Create(typeof(TripleParameterType<int, string, int>));
            // Verify outcome
            Assert.AreEqual(1, result.Parameter1, "Parameter1");
            Assert.AreEqual("parameter22", result.Parameter2, "Parameter2");
            Assert.AreEqual(3, result.Parameter3, "Parameter3");
            // Teardown
        }

        private static DefaultSpecimenContainer CreateContainer()
        {
            var builder = new CompositeSpecimenBuilder(
                new Int32SequenceGenerator(),
                new StringGenerator(() => Guid.NewGuid()),
                new DecimalSequenceGenerator(),
                new BooleanSwitch(),
                new GuidGenerator(),
                new ModestConstructorInvoker(),
                new ParameterRequestTranslator(),
                new StringSeedUnwrapper(),
                new ValueIgnoringSeedUnwrapper());
            return new DefaultSpecimenContainer(builder);
        }
    }
}
