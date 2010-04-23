using System;
using System.IO;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace Ploeh.AutoFixtureUnitTest.Kernel
{
    public class BuildTraceWriterTest
    {
        [Fact]
        public void SutIsSpecimenBuilder()
        {
            // Fixture setup
            var dummyWriter = TextWriter.Null;
            var dummyBuilder = new DelegatingTracingBuilder();
            // Exercise system
            var sut = new BuildTraceWriter(dummyWriter, dummyBuilder);
            // Verify outcome
            Assert.IsAssignableFrom<ISpecimenBuilder>(sut);
            // Teardown
        }

        [Fact]
        public void CreateWithNullWriterWillThrow()
        {
            // Fixture setup
            var dummyBuilder = new DelegatingTracingBuilder();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new BuildTraceWriter(null, dummyBuilder));
            // Teardown
        }

        [Fact]
        public void CreateWithNullBuilderWillThrow()
        {
            // Fixture setup
            var dummyWriter = TextWriter.Null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new BuildTraceWriter(dummyWriter, null));
            // Teardown
        }

        [Fact]
        public void CreateWillReturnCorrectResult()
        {
            // Fixture setup
            var expectedSpecimen = new object();
            var stubBuilder = new TracingBuilder(new DelegatingSpecimenBuilder { OnCreate = (r, c) => expectedSpecimen });

            var dummyWriter = TextWriter.Null;
            var sut = new BuildTraceWriter(dummyWriter, stubBuilder);
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContainer();
            var result = sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.Equal(expectedSpecimen, result);
            // Teardown
        }

        [Fact]
        public void CreateWillInvokeDecoratedBuilderWithCorrectParameters()
        {
            // Fixture setup
            var expectedRequest = new object();
            var expectedContainer = new DelegatingSpecimenContainer();

            var verified = false;
            var mockBuilder = new TracingBuilder(new DelegatingSpecimenBuilder { OnCreate = (r, c) => verified = expectedRequest == r && expectedContainer == c });

            var dummyWriter = TextWriter.Null;
            var sut = new BuildTraceWriter(dummyWriter, mockBuilder);
            // Exercise system
            sut.Create(expectedRequest, expectedContainer);
            // Verify outcome
            Assert.True(verified, "Mock verified");
            // Teardown
        }

        [Fact]
        public void SpecimenRequestedWillWriteCorrectMessageToWriter()
        {
            // Fixture setup
            var writer = new StringWriter();
            var builder = new DelegatingTracingBuilder();

            var depth = new Random().Next(1, 10);
            var request = new object();

            var sut = new BuildTraceWriter(writer, builder);
            // Exercise system
            builder.RaiseSpecimenRequested(new SpecimenTraceEventArgs(request, depth));
            // Verify outcome
            var expected = new string(' ', depth * 2) + "Requested: " + request + Environment.NewLine;
            Assert.Equal(expected, writer.ToString());
            // Teardown
        }

        [Fact]
        public void SpecimenCreatedWillWriteCorrectMessageToWriter()
        {
            // Fixture setup
            var writer = new StringWriter();
            var builder = new DelegatingTracingBuilder();

            var depth = new Random().Next(1, 10);
            var specimen = new object();

            var sut = new BuildTraceWriter(writer, builder);
            // Exercise system
            var dummyRequest = new object();
            builder.RaiseSpecimenCreated(new SpecimenCreatedEventArgs(dummyRequest, specimen, depth));
            // Verify outcome
            var expected = new string(' ', depth * 2) + "Created: " + specimen + Environment.NewLine;
            Assert.Equal(expected, writer.ToString());
            // Teardown
        }

        [Fact]
        public void RequestFormatterIsGivenCorrectTextWriter()
        {
            // Fixture setup
            var expectedWriter = new StringWriter();
            var sut = new BuildTraceWriter(expectedWriter, new DelegatingTracingBuilder());

            bool verified = false;
            sut.TraceRequestFormatter = (tw, r, i) => verified = tw == expectedWriter;
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContainer();
            sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void SpecimenFormatterIsGivenCorrectTextWriter()
        {
            // Fixture setup
            var expectedWriter = new StringWriter();
            var sut = new BuildTraceWriter(expectedWriter, new DelegatingTracingBuilder());

            bool verified = false;
            sut.TraceCreatedSpecimenFormatter = (tw, r, i) => verified = tw == expectedWriter;
            // Exercise system
            var dummyRequest = new object();
            var dummyContainer = new DelegatingSpecimenContainer();
            sut.Create(dummyRequest, dummyContainer);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Fact]
        public void AssignNullRequestFormatterWillThrow()
        {
            // Fixture setup
            var sut = new BuildTraceWriter(TextWriter.Null, new DelegatingTracingBuilder());
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.TraceRequestFormatter = null);
            // Teardown
        }

        [Fact]
        public void AssignNullSpecimenFormatterWillThrow()
        {
            // Fixture setup
            var sut = new BuildTraceWriter(TextWriter.Null, new DelegatingTracingBuilder());
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.TraceCreatedSpecimenFormatter = null);
            // Teardown
        }

        [Fact]
        public void RequestFormatterIsProperWritableProperty()
        {
            // Fixture setup
            Action<TextWriter, object, int> expected = (tw, r, i) => { };
            var sut = new BuildTraceWriter(TextWriter.Null, new DelegatingTracingBuilder());
            // Exercise system
            sut.TraceRequestFormatter = expected;
            Action<TextWriter, object, int> result = sut.TraceRequestFormatter;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void SpecimenFormatterIsProperWritableProperty()
        {
            // Fixture setup
            Action<TextWriter, object, int> expected = (tw, r, i) => { };
            var sut = new BuildTraceWriter(TextWriter.Null, new DelegatingTracingBuilder());
            // Exercise system
            sut.TraceCreatedSpecimenFormatter = expected;
            Action<TextWriter, object, int> result = sut.TraceCreatedSpecimenFormatter;
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }
    }
}