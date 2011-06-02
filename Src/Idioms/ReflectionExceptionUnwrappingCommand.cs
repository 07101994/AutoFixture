﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Idioms
{
    public class ReflectionExceptionUnwrappingCommand : IContextualCommand
    {
        private IContextualCommand command;

        public ReflectionExceptionUnwrappingCommand(IContextualCommand command)
        {
            this.command = command;
        }

        public IContextualCommand Command
        {
            get { return this.command; }
        }

        #region IContextualCommand Members

        public void Execute()
        {
            this.Command.Execute();
        }

        #endregion
    }
}
