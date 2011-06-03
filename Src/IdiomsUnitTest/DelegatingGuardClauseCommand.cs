﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Idioms;
using System.Reflection;

namespace Ploeh.AutoFixture.IdiomsUnitTest
{
    public class DelegatingGuardClauseCommand : IGuardClauseCommand
    {
        public DelegatingGuardClauseCommand()
        {
            this.ContextType = typeof(object);
            this.MemberInfo = typeof(object).GetMembers().First();
            this.OnExecute = v => { };
            this.OnThrow = v => new Exception();
            this.OnThrowWithInner = (v, e) => { };
        }

        public Action<object> OnExecute { get; set; }

        public Func<string, Exception> OnThrow { get; set; }

        public Action<string, Exception> OnThrowWithInner { get; set; }

        #region IContextualCommand Members

        public MemberInfo MemberInfo { get; set; }

        public Type ContextType { get; set; }

        public void Execute(object value)
        {
            this.OnExecute(value);
        }

        public Exception Throw(string value)
        {
            return this.OnThrow(value);
        }

        public void Throw(string value, Exception innerException)
        {
            this.OnThrowWithInner(value, innerException);
        }

        #endregion
    }
}
