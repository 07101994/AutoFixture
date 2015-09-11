﻿using System;
using System.Reflection;

namespace Ploeh.AutoFixture.Kernel
{
    public class PropertyTypeAndNameCriterion : IEquatable<PropertyInfo>
    {
        private readonly IEquatable<Type> typeCriterion;
        private readonly IEquatable<string> nameCriterion;

        public PropertyTypeAndNameCriterion(
            IEquatable<Type> typeCriterion,
            IEquatable<string> nameCriterion)
        {
            if (typeCriterion == null)
                throw new ArgumentNullException("typeCriterion");
            if (nameCriterion == null)
                throw new ArgumentNullException("nameCriterion");

            this.typeCriterion = typeCriterion;
            this.nameCriterion = nameCriterion;
        }

        public bool Equals(PropertyInfo other)
        {
            if (other == null)
                return false;

            return this.typeCriterion.Equals(other.PropertyType)
                && this.nameCriterion.Equals(other.Name);
        }
    }
}