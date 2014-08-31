using System.Collections.Generic;

namespace Ploeh.AutoFixture.AutoNSubstitute.UnitTest.TestTypes
{
    public class TypeWithSignatureMethods
    {
        public static void VoidMethodWithString(string arg)
        {
        }

        public static void MethodWithString(string arg)
        {
        }
        public static void MethodWithStringArray(string[] arg)
        {
        }
        public static void MethodWithGenerics<T>(T arg)
        {
        }
        public static void MethodWithEnumerable<T>(IEnumerable<T> arg)
        {
        }
        public static void MethodWithArray<T>(T[] arg)
        {
        }
        public static void MethodWithOptionalParameter<T>(T arg)
        {
        }
        public static void MethodWithParamsParameter<T>(T arg)
        {
        }
        public static void MethodWithSameName(string arg)
        {
        }
    }
}