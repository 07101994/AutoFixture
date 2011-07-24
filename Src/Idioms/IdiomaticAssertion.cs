﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ploeh.AutoFixture.Idioms
{
    /// <summary>
    /// Base implementation of <see cref="IIdiomaticAssertion" />.
    /// </summary>
    /// <remarks>
    /// <para>
    /// IdiomaticAssertion provides default implementations of all methods of
    /// <see cref="IIdiomaticAssertion" />, making sure that higher-order methods call into
    /// lower-order methods; e.g. that <see cref="Verify(Assembly)" /> calls
    /// <see cref="Verify(Type[])" /> with all public types in the assembly.
    /// </para>
    /// <para>
    /// Implementers can override the appropriate methods instead of creating an implementation of
    /// IIdiomaticAssertion completely from scratch.
    /// </para>
    /// </remarks>
    public abstract class IdiomaticAssertion : IIdiomaticAssertion
    {
        #region IIdiomaticAssertion Members

        /// <summary>
        /// Calls <see cref="Verify(Assembly)" /> for each Assembly in
        /// <paramref name="assemblies" />.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public virtual void Verify(params Assembly[] assemblies)
        {
            foreach (var a in assemblies)
            {
                this.Verify(a);
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(Assembly)" /> for each Assembly in
        /// <paramref name="assemblies" />.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public virtual void Verify(IEnumerable<Assembly> assemblies)
        {
            this.Verify(assemblies.ToArray());
        }

        /// <summary>
        /// Calls <see cref="Verify(Type[])" /> for each public Type in
        /// <paramref name="assembly" />.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public virtual void Verify(Assembly assembly)
        {
            this.Verify(assembly.GetExportedTypes());
        }

        /// <summary>
        /// Calls <see cref="Verify(Type)" /> for each Type in <paramref name="types" />.
        /// </summary>
        /// <param name="types">The types.</param>
        public virtual void Verify(params Type[] types)
        {
            foreach (var t in types)
            {
                this.Verify(t);
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(Type)" /> for each Type in <paramref name="types" />.
        /// </summary>
        /// <param name="types">The types.</param>
        public virtual void Verify(IEnumerable<Type> types)
        {
            this.Verify(types.ToArray());
        }

        /// <summary>
        /// Calls <see cref="Verify(ConstructorInfo[])" />, <see cref="Verify(MethodInfo[])" /> and
        /// <see cref="Verify(PropertyInfo[])" /> for each constructor, method and property in
        /// <paramref name="type" />.
        /// </summary>
        /// <param name="type">The type.</param>
        public virtual void Verify(Type type)
        {
            this.Verify(type.GetConstructors());
            this.Verify(IdiomaticAssertion.GetMethodsExceptPropertyAccessors(type));
            this.Verify(type.GetProperties());
        }

        /// <summary>
        /// Calls <see cref="Verify(MemberInfo)" /> for each member in
        /// <paramref name="memberInfos" />.
        /// </summary>
        /// <param name="memberInfos">The members.</param>
        public virtual void Verify(params MemberInfo[] memberInfos)
        {
            foreach (var m in memberInfos)
            {
                this.Verify(m);
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(MemberInfo)" /> for each member in
        /// <paramref name="memberInfos" />.
        /// </summary>
        /// <param name="memberInfos">The members.</param>
        public virtual void Verify(IEnumerable<MemberInfo> memberInfos)
        {
            this.Verify(memberInfos.ToArray());
        }

        /// <summary>
        /// Calls <see cref="Verify(ConstructorInfo)" />, <see cref="MethodInfo" /> or
        /// <see cref="PropertyInfo" />, depending on the subtype of
        /// <paramref name="memberInfo" />.
        /// </summary>
        /// <param name="memberInfo">The member.</param>
        public virtual void Verify(MemberInfo memberInfo)
        {
            var c = memberInfo as ConstructorInfo;
            if (c != null)
            {
                this.Verify(c);
                return;
            }

            var m = memberInfo as MethodInfo;
            if (m != null)
            {
                this.Verify(m);
                return;
            }

            var p = memberInfo as PropertyInfo;
            if (p != null)
            {
                this.Verify(p);
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(ConstructorInfo)" /> for each ConstructorInfo in
        /// <paramref name="constructorInfos" />.
        /// </summary>
        /// <param name="constructorInfos">The constructors.</param>
        public virtual void Verify(params ConstructorInfo[] constructorInfos)
        {
            foreach (var c in constructorInfos)
            {
                this.Verify(c);
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(ConstructorInfo)" /> for each ConstructorInfo in
        /// <paramref name="constructorInfos" />.
        /// </summary>
        /// <param name="constructorInfos">The constructors.</param>
        public virtual void Verify(IEnumerable<ConstructorInfo> constructorInfos)
        {
            this.Verify(constructorInfos.ToArray());
        }

        /// <summary>
        /// Does nothing. Override to implement.
        /// </summary>
        /// <param name="constructorInfo">The constructor.</param>
        public virtual void Verify(ConstructorInfo constructorInfo)
        {
        }

        /// <summary>
        /// Calls <see cref="Verify(MethodInfo)" /> for each MethodInfo in
        /// <paramref name="methodInfos" />.
        /// </summary>
        /// <param name="methodInfos">The methods.</param>
        public virtual void Verify(params MethodInfo[] methodInfos)
        {
            foreach (var m in methodInfos)
            {
                this.Verify(m);
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(MethodInfo)" /> for each MethodInfo in
        /// <paramref name="methodInfos" />.
        /// </summary>
        /// <param name="methodInfos">The methods.</param>
        public virtual void Verify(IEnumerable<MethodInfo> methodInfos)
        {
            this.Verify(methodInfos.ToArray());
        }

        /// <summary>
        /// Does nothing. Override to implement.
        /// </summary>
        /// <param name="methodInfo">The method.</param>
        public virtual void Verify(MethodInfo methodInfo)
        {
        }

        /// <summary>
        /// Calls <see cref="Verify(PropertyInfo)" /> for each PropertyInfo in
        /// <paramref name="propertyInfos" />.
        /// </summary>
        /// <param name="propertyInfos">The properties.</param>
        public virtual void Verify(params PropertyInfo[] propertyInfos)
        {
            foreach (var p in propertyInfos)
            {
                this.Verify(p);
            }
        }

        /// <summary>
        /// Calls <see cref="Verify(PropertyInfo)" /> for each PropertyInfo in
        /// <paramref name="propertyInfos" />.
        /// </summary>
        /// <param name="propertyInfos">The properties.</param>
        public virtual void Verify(IEnumerable<PropertyInfo> propertyInfos)
        {
            this.Verify(propertyInfos.ToArray());
        }

        /// <summary>
        /// Does nothing. Override to implement.
        /// </summary>
        /// <param name="propertyInfo">The property.</param>
        public virtual void Verify(PropertyInfo propertyInfo)
        {
        }

        #endregion

        private static IEnumerable<MethodInfo> GetMethodsExceptPropertyAccessors(Type type)
        {
            return type.GetMethods().Except(type.GetProperties().SelectMany(p => p.GetAccessors()));
        }
    }
}
