﻿namespace Ploeh.AutoFixture
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Kernel;

    public class LambdaExpressionGenerator : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var requestType = request as Type;
            if (requestType == null)
            {
                return new NoSpecimen();
            }

            if (requestType.BaseType != typeof(LambdaExpression))
            {
                return new NoSpecimen();
            }

            var delegateType = requestType.GetGenericArguments().Single();
            var genericArguments = delegateType.GetGenericArguments().Select(Expression.Parameter).ToList();

            if (delegateType.Name.StartsWith( "Action"))
            {
                return Expression.Lambda(Expression.Empty(), genericArguments);
            }

            var body = genericArguments.Last();
            var parameters = new List<ParameterExpression>();
            if (genericArguments.Count > 1)
            {
                parameters = genericArguments.Take(genericArguments.Count - 1).ToList();
            }

            return Expression.Lambda(body, parameters);
        }
    }
}