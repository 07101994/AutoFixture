﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Represents an error about a missing Guard Clause.
    /// </summary>
    public class GuardClauseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuardClauseException"/> class.
        /// </summary>
        public GuardClauseException()
            : base("An invariant was not correctly protected. Are you missing a Guard Clause?")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardClauseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public GuardClauseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuardClauseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public GuardClauseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
