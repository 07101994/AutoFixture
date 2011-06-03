﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

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

        public MemberInfo MemberInfo
        {
            get { return this.command.MemberInfo; }
        }

        public Type ContextType
        {
            get { return this.command.ContextType; }
        }

        public void Execute(object value)
        {
            try
            {
                this.Command.Execute(value);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        public void Throw(string value)
        {
        }

        public void Throw(string value, Exception innerException)
        {
        }

        #endregion
    }
}
