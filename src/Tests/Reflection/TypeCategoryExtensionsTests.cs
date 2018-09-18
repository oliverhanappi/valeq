using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NUnit.Framework;
using Valeq.Reflection;

namespace Tests.Reflection
{
    [TestFixture]
    public class TypeCategoryExtensionsTests
    {
        // Type Overview: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/types
        
        // Value Types
        [TestCase(typeof(bool), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(bool?), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(byte), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(byte?), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(sbyte), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(sbyte?), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(char), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(char?), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(decimal), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(decimal?), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(double), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(double?), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(float), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(float?), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(int), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(int?), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(uint), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(uint?), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(long), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(long?), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(ulong), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(ulong?), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(short), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(short?), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(ushort), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(ushort?), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(void), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(DateTime), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(TimeSpan), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(StringComparison), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(TestEnum), ExpectedResult = TypeCategory.Simple)]
        // Reference Types
        [TestCase(typeof(string), ExpectedResult = TypeCategory.Simple)]
        [TestCase(typeof(object), ExpectedResult = TypeCategory.Complex)]
        [TestCase(typeof(TestStruct), ExpectedResult = TypeCategory.Complex)]
        [TestCase(typeof(TestClass), ExpectedResult = TypeCategory.Complex)]
        [TestCase(typeof(ITestInterface), ExpectedResult = TypeCategory.Complex)]
        [TestCase(typeof(TestDelegate), ExpectedResult = TypeCategory.Simple)]
        // Collections
        [TestCase(typeof(IEnumerable), ExpectedResult = TypeCategory.Collection)]
        [TestCase(typeof(IEnumerable<object>), ExpectedResult = TypeCategory.Collection)]
        [TestCase(typeof(IEnumerable<int>), ExpectedResult = TypeCategory.Collection)]
        [TestCase(typeof(IEnumerable<DateTime>), ExpectedResult = TypeCategory.Collection)]
        [TestCase(typeof(IEnumerable<TestClass>), ExpectedResult = TypeCategory.Collection)]
        [TestCase(typeof(ConcurrentDictionary<TestClass, TestStruct>), ExpectedResult = TypeCategory.Collection)]
        [TestCase(typeof(int[]), ExpectedResult = TypeCategory.Collection)]
        [TestCase(typeof(TestStruct[]), ExpectedResult = TypeCategory.Collection)]
        [TestCase(typeof(TestClass[]), ExpectedResult = TypeCategory.Collection)]
        public TypeCategory GetCategory(Type type)
        {
            return type.GetCategory();
        }

        public struct TestStruct
        {
        }

        public class TestClass
        {
        }

        public interface ITestInterface
        {
        }

        public enum TestEnum
        {
        }

        public delegate void TestDelegate();
    }
}
