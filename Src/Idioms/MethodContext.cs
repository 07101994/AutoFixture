﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using Ploeh.AutoFixture.Kernel;

namespace Ploeh.AutoFixture.Idioms
{
    public class MethodContext : IVerifiableBoundary
    {
        private readonly ISpecimenBuilderComposer composer;
        private readonly MethodInfo methodInfo;

        public MethodContext(ISpecimenBuilderComposer composer, MethodInfo methodInfo)
        {
            if (composer == null)
            {
                throw new ArgumentNullException("composer");
            }
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }

            this.composer = composer;
            this.methodInfo = methodInfo;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ISpecimenBuilderComposer Composer
        {
            get { return this.composer; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public MethodInfo MethodInfo
        {
            get { return this.methodInfo; }
        }

        #region IVerifiableBoundary Members

        public void VerifyBoundaries(IBoundaryConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            var builder = this.Composer.Compose();
            

            var parameterValues = (from p in this.MethodInfo.GetParameters()
                                   select new { Parameter = p, Value = new SpecimenContext(builder).Resolve(p) }).ToDictionary(x => x.Parameter, x => x.Value);

            var combinations = from p in parameterValues.Keys
                               from b in convention.CreateBoundaryBehaviors(p.ParameterType)
                               select new { Parameter = p, Behavior = MethodContext.WrapBehavior(b) };
            foreach (var c in combinations)
            {
                Action<object> invokeMethod = x =>
                    {
                        var values = new Dictionary<ParameterInfo, object>(parameterValues);
                        values[c.Parameter] = x;

                        var specimen = new SpecimenContext(builder).Resolve(this.MethodInfo.ReflectedType);
                        this.MethodInfo.Invoke(specimen, values.Values.ToArray());
                    };

                c.Behavior.Assert(invokeMethod);
            }
        }

        #endregion

#warning Refactor to DRY
        private static IBoundaryBehavior WrapBehavior(IBoundaryBehavior behavior)
        {
            var exceptionBehavior = behavior as ExceptionBoundaryBehavior;
            if (exceptionBehavior == null)
            {
                return behavior;
            }

            return new ReflectionExceptionBoundaryBehavior(exceptionBehavior);
        }
    }
}
