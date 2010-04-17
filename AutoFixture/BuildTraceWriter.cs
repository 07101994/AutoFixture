namespace Ploeh.AutoFixture
{
	using System;
	using System.IO;
	using Kernel;

    /// <summary>
    /// Trace writer that will write out a trace of object requests and created objects
    /// in the <see cref="ISpecimenBuilder" /> pipeline.
    /// </summary>
	public class BuildTraceWriter : RequestTracker
	{
		private TextWriter outputStream;
		private int indentLevel = 0;

        /// <summary>
        /// Gets or sets the formatter for tracing a request.
        /// </summary>
        /// <value>The request trace formatter.</value>
		public Func<object, int, string> TraceRequestFormatter
		{
			get;
			set;
		}

        /// <summary>
        /// Gets or sets the formatter for tracing a created specimen.
        /// </summary>
        /// <value>The created specimen trace formatter.</value>
		public Func<object, int, string> TraceCreatedSpecimenFormatter
		{
			get;
			set;
		}

        /// <summary>
        /// Gets the current indent level of the trace.
        /// </summary>
        /// <value>The indent level.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Performance",
            "CA1811:AvoidUncalledPrivateCode",
            Justification = "This property is currently used by tests, and will probably be used upstream when development progresses.")]
        public int IndentLevel
		{
			get;
			private set;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildTraceWriter"/> class.
        /// </summary>
        /// <param name="outStream">The output stream for the trace.</param>
		public BuildTraceWriter(TextWriter outStream)
		{
			outputStream = outStream;
			TraceRequestFormatter = (obj, i) => new string(' ', i * 2) + obj.ToString();
            TraceCreatedSpecimenFormatter = (obj, i) => new string(' ', i * 2) + obj.ToString();
		}

        /// <summary>
        /// Invoked when a request is tracked. Writes a trace of the request.
        /// </summary>
        /// <param name="request">The request.</param>
		protected override void TrackRequest(object request)
		{
			outputStream.WriteLine(TraceRequestFormatter(request, indentLevel));
			indentLevel++;
		}

        /// <summary>
        /// Invoked when a created specimen is tracked. Writes a trace of the response.
        /// </summary>
        /// <param name="specimen">The specimen.</param>
		protected override void TrackCreatedSpecimen(object specimen)
		{
			indentLevel--;
			outputStream.WriteLine(TraceCreatedSpecimenFormatter(specimen, indentLevel));
		}
	}
}