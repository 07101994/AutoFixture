﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.TestTypeFoundation;
using Ploeh.SemanticComparison.Fluent;

namespace Ploeh.SemanticComparison.UnitTest
{
    [TestClass]
    public class LikenessTest
    {
        public LikenessTest()
        {
        }

        [TestMethod]
        public void CreateWithNullValueWillHoldCorrectValue()
        {
            // Fixture setup
            var sut = new Likeness<PropertyHolder<string>, FieldHolder<int>>(null);
            // Exercise system
            PropertyHolder<string> result = sut.Value;
            // Verify outcome
            Assert.IsNull(result, "Value");
            // Teardown
        }

        [TestMethod]
        public void ValueIsCorrect()
        {
            // Fixture setup
            var expectedValue = new ConcreteType();
            var sut = new Likeness<ConcreteType, PropertyHolder<double>>(expectedValue);
            // Exercise system
            ConcreteType result = sut.Value;
            // Verify outcome
            Assert.AreEqual(expectedValue, result, "Value");
            // Teardown
        }

        [TestMethod]
        public void SutIsEquatable()
        {
            // Fixture setup
            // Exercise system
            var sut = new Likeness<int, string>(1);
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(IEquatable<string>));
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new Likeness<DateTime, TimeSpan>(DateTime.Now);
            object nullObject = null;
            // Exercise system
            var result = sut.Equals(nullObject);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualNullValue()
        {
            // Fixture setup
            var sut = new Likeness<PropertyHolder<short>, PropertyHolder<string>>(new PropertyHolder<short>());
            PropertyHolder<string> nullValue = null;
            // Exercise system
            var result = sut.Equals(nullValue);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsNullValueWhenSourceIsNull()
        {
            // Fixture setup
            var sut = new Likeness<PropertyHolder<string>, PropertyHolder<string>>(null);
            PropertyHolder<string> nullValue = null;
            // Exercise system
            var result = sut.Equals(nullValue);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsNullObjectWhenSourceIsNull()
        {
            // Fixture setup
            var sut = new Likeness<PropertyHolder<string>, PropertyHolder<string>>(null);
            object nullValue = null;
            // Exercise system
            var result = sut.Equals(nullValue);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutWithValueDoesNotEqualNullValue()
        {
            // Fixture setup
            var sut = new Likeness<ConcreteType, AbstractType>(new ConcreteType());
            ConcreteType nullValue = null;
            // Exercise system
            var result = sut.Equals(nullValue);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutWithValueDoesNotEqualNullObject()
        {
            // Fixture setup
            var sut = new Likeness<ConcreteType, AbstractType>(new ConcreteType());
            object nullObject = null;
            // Exercise system
            var result = sut.Equals(nullObject);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsItself()
        {
            // Fixture setup
            var sut = new Likeness<ConcreteType, AbstractType>(new ConcreteType());
            // Exercise system
            var result = sut.Equals(sut);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void GetHashCodeShouldReturnHashCodeOfContainedObject()
        {
            // Fixture setup
            var anonymousDateTime = new DateTime(2010, 1, 3);
            int expectedHashCode = anonymousDateTime.GetHashCode();

            var sut = new Likeness<DateTime, DateTimeOffset>(anonymousDateTime);
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.AreEqual(expectedHashCode, result, "GetHashCode");
            // Teardown
        }

        [TestMethod]
        public void GetHashCodeWhenNullIsContainedWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Likeness<string, string>(null);
            // Exercise system
            var result = sut.GetHashCode();
            // Verify outcome
            Assert.AreEqual(0, result, "GetHashCode");
            // Teardown
        }

        [TestMethod]
        public void ToStringReturnsCorrectResult()
        {
            // Fixture setup
            var anonymousTimeSpan = new TimeSpan(7, 4, 2, 1);
            string expectedText = "Likeness of " + anonymousTimeSpan.ToString();

            var sut = new Likeness<TimeSpan, TimeZoneInfo>(anonymousTimeSpan);
            // Exercise system
            var result = sut.ToString();
            // Verify outcome
            Assert.AreEqual(expectedText, result, "ToString");
            // Teardown
        }

        [TestMethod]
        public void ToStringOfContainedNullWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new Likeness<string, string>(null);
            // Exercise system
            var result = sut.ToString();
            // Verify outcome
            Assert.AreEqual("Likeness of null", result, "ToString");
            // Teardown
        }

        [TestMethod]
        public void SutDoesNotEqualAnonymousObject()
        {
            // Fixture setup
            var sut = new Likeness<ConcreteType, ConcreteType>(new ConcreteType());
            object anonymousObject = new object();
            // Exercise system
            var result = sut.Equals(anonymousObject);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsIdenticalStrongType()
        {
            // Fixture setup
            var ticks = 8293247;
            var value = TimeSpan.FromTicks(ticks);
            var sut = new Likeness<TimeSpan, TimeSpan>(value);
            var other = TimeSpan.FromTicks(ticks);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutEqualsIdenticalWeakType()
        {
            // Fixture setup
            var ticks = 8293247;
            var value = TimeSpan.FromTicks(ticks);
            var sut = new Likeness<TimeSpan, TimeSpan>(value);
            object other = TimeSpan.FromTicks(ticks);
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void ComparingStringPropertyHolderLikenessToRealStringPropertyHolderWillIndicateEquality()
        {
            // Fixture setup
            string anonymousText = "Anonymous text";

            PropertyHolder<string> likenObject = new PropertyHolder<string>();
            likenObject.Property = anonymousText;

            PropertyHolder<string> comparee = new PropertyHolder<string>();
            comparee.Property = anonymousText;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, true);
        }

        [TestMethod]
        public void ComparingStringPropertyHoldersWithDifferentValuesWillIndicateDifference()
        {
            // Fixture setup
            string anonymousText1 = "Anonymous text";
            string anonymousText2 = "Some other text";

            PropertyHolder<string> likenObject = new PropertyHolder<string>();
            likenObject.Property = anonymousText1;

            PropertyHolder<string> comparee = new PropertyHolder<string>();
            comparee.Property = anonymousText2;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [TestMethod]
        public void ComparingStringFieldHolderLikenessToRealStringFieldHolderWillIndicateEquality()
        {
            // Fixture setup
            string anonymousText = "Anonymous text";

            FieldHolder<string> likenObject = new FieldHolder<string>();
            likenObject.Field = anonymousText;

            FieldHolder<string> comparee = new FieldHolder<string>();
            comparee.Field = anonymousText;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, true);
        }

        [TestMethod]
        public void ComparingStringFieldHoldersWithDifferentValuesWillIndicateDifference()
        {
            // Fixture setup
            string anonymousText1 = "Anonymous text";
            string anonymousText2 = "Some other text";

            FieldHolder<string> likenObject = new FieldHolder<string>();
            likenObject.Field = anonymousText1;

            FieldHolder<string> comparee = new FieldHolder<string>();
            comparee.Field = anonymousText2;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [TestMethod]
        public void CompareAnonymousTypeLikenessToStringFieldHolderWillIndicateEqualityWhenValuesAreEqual()
        {
            // Fixture setup
            string anonymousText = "Anonymou text";

            var likenObject = new
            {
                Field = anonymousText
            };

            FieldHolder<string> comparee = new FieldHolder<string>();
            comparee.Field = anonymousText;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, true);
        }

        [TestMethod]
        public void CompareAnonymousTypeLikenessToStringPropertyHolderWillIndicateDifferenceWhenValuesAreDifferent()
        {
            // Fixture setup
            string anonymousText1 = "Anonymous text";
            string anonymousText2 = "Some other text";

            var likenObject = new
            {
                Field = anonymousText1
            };

            FieldHolder<string> comparee = new FieldHolder<string>();
            comparee.Field = anonymousText2;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [TestMethod]
        public void ObjectsWithNullPropertiesWillHaveLikeness()
        {
            // Fixture setup
            PropertyHolder<object> likenObject = new PropertyHolder<object>();
            likenObject.Property = null;

            PropertyHolder<object> comparee = new PropertyHolder<object>();
            comparee.Property = null;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, true);
        }

        [TestMethod]
        public void LikenessOfObjectWithNullPropertyWillNotBeEqualToObjectWithValuedProperty()
        {
            // Fixture setup
            PropertyHolder<object> likenObject = new PropertyHolder<object>();
            likenObject.Property = null;

            PropertyHolder<object> comparee = new PropertyHolder<object>();
            comparee.Property = new object();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [TestMethod]
        public void LikenessOfObjectWithValuePropertyWillNotBeEqualToObjectWithNullProperty()
        {
            // Fixture setup
            PropertyHolder<object> likenObject = new PropertyHolder<object>();
            likenObject.Property = new object();

            PropertyHolder<object> comparee = new PropertyHolder<object>();
            comparee.Property = null;

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [TestMethod]
        public void LikenessOfObjectWithPropertyWillNotBeEqualToPropertyWithDifferentProperty()
        {
            // Fixture setup
            var likenObject = new { SomeOtherProperty = new object() };

            PropertyHolder<object> comparee = new PropertyHolder<object>();
            comparee.Property = new object();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [TestMethod]
        public void LikenessAgainstObjectWithOverloadedMembersWillNotThrow()
        {
            // Fixture setup
            var likenObject = new object();

            var comparee = new TypeWithOverloadedMembers();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [TestMethod]
        public void LikenessAgainstObjectWithIndexerWillNotThrow()
        {
            // Fixture setup
            var likenObject = new object();

            var comparee = new TypeWithIndexer();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [TestMethod]
        public void LikenessAgainstObjectWithHidingPropertyWillNotThrow()
        {
            // Fixture setup
            var likenObject = new A();

            var comparee = new B();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [TestMethod]
        public void LikenessOfObjectWithHidingPropertyWillNotThrow()
        {
            // Fixture setup
            var likenObject = new B();

            var comparee = new A();

            // The rest of the test
            LikenessTest.CompareLikenessToObject(likenObject, comparee, false);
        }

        [TestMethod]
        public void SutOfAbstractTypeEqualsConcreteInstancesThatDifferOnlyOnMemberNotDefinedByAbstraction()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property5 = "Ploeh";
            var sut = new Likeness<ConcreteType, AbstractType>(value);

            var other = new ConcreteType();
            other.Property1 = value.Property1;
            other.Property2 = value.Property2;
            other.Property3 = value.Property3;
            other.Property4 = value.Property4;
            other.Property5 = "Fnaah";
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutOfAbstractTypeDoesNotEqualConcreteInstanceWhenPropertyDiffers()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property4 = "Ploeh";
            var sut = new Likeness<ConcreteType, AbstractType>(value);

            var other = new ConcreteType();
            other.Property1 = value.Property1;
            other.Property2 = value.Property2;
            other.Property3 = value.Property3;
            other.Property4 = "Fnaah";
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void EqualsIsTrueWhenCorrectMappingHasBeenDefined()
        {
            // Fixture setup
            var value = new PropertyHolder<string>();
            value.Property = "Foo";
            var sut = new Likeness<PropertyHolder<string>, FieldHolder<string>>(value)
                .With(d => d.Field).EqualsWhen((s, d) => s.Property == d.Field);

            var other = new FieldHolder<string>();
            other.Field = value.Property;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutWithoutPropertyEqualsInstanceEvenIfItDiffersOnThatProperty()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property5 = "amet";
            var sut = new Likeness<ConcreteType, ConcreteType>(value).Without(x => x.Property5);

            var other = new ConcreteType();
            other.Property1 = value.Property1;
            other.Property2 = value.Property2;
            other.Property3 = value.Property3;
            other.Property4 = value.Property4;
            other.Property5 = "Fnaah";
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutWithoutTwoPropertiesEqualsInstanceEvenThoughThosePropertyValuesDiffer()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            value.Property5 = "amet";
            var sut = new Likeness<ConcreteType, ConcreteType>(value).Without(x => x.Property5).Without(x => x.Property1);

            var other = new ConcreteType();
            other.Property1 = "Ndøh";
            other.Property2 = value.Property2;
            other.Property3 = value.Property3;
            other.Property4 = value.Property4;
            other.Property5 = "Sqryt";
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutWithSemanticEqualityWillReturnTrue()
        {
            // Fixture setup
            var value = new ConcreteType("Lorem", "ipsum", "dolor", "sit");
            var sut = new Likeness<ConcreteType, DoublePropertyHolder<object, object>>(value);

            var other = new DoublePropertyHolder<object, object>();
            other.Property1 = value.Property1;
            other.Property2 = value.Property2;
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutWithValueThatDoesNotMatchTypeParameterWillReturnFalseEvenWhenComparedWithIdenticalValue()
        {
            // Fixture setup
            var value = new { Property1 = "Lorem", Ploeh = "ipsum" };
            var sut = value.AsSource().OfLikeness<DoublePropertyHolder<object, object>>();

            var other = new { Property1 = "Lorem", Ploeh = "ipsum" };
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutWithAnonymousValueWillEqualSemanticallyIdenticalValue()
        {
            // Fixture setup
            var value = new { Property1 = "Lorem", Property2 = "ipsum" };
            var sut = value.AsSource().OfLikeness<DoublePropertyHolder<object, object>>();

            var other = new DoublePropertyHolder<object, object> { Property1 = value.Property1, Property2 = value.Property2 };
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void SutWithAnonymousSourceAndMemberMappingWillEqualMappedIdenticalValue()
        {
            // Fixture setup
            var value = new { Property1 = "Lorem", Ploeh = "ipsum", Fnaah = "dolor" };
            var sut = value.AsSource().OfLikeness<DoubleFieldHolder<string, string>>()
                .With(d => d.Field1).EqualsWhen((s, d) => s.Property1 == d.Field1)
                .With(d => d.Field2).EqualsWhen((s, d) => s.Ploeh == d.Field2);

            var other = new DoubleFieldHolder<string, string> { Field1 = value.Property1, Field2 = value.Ploeh };
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void EqualsWhenOverriddenSourcePropertyEqualsReturnsTrueWillReturnTrue()
        {
            // Fixture setup
            var equalityResponse = true;
            var value = new PropertyHolder<EqualityResponder>();
            value.Property = new EqualityResponder(equalityResponse);
            var sut = new Likeness<PropertyHolder<EqualityResponder>, PropertyHolder<object>>(value);

            var other = new PropertyHolder<object>();
            other.Property = new object();
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.AreEqual(equalityResponse, result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void EqualsWhenOverriddenSourcePropertyEqualsReturnsFalseWillReturnFalse()
        {
            // Fixture setup
            var equalityResponse = false;
            var value = new PropertyHolder<EqualityResponder>();
            value.Property = new EqualityResponder(equalityResponse);
            var sut = new Likeness<PropertyHolder<EqualityResponder>, PropertyHolder<object>>(value);

            var other = new PropertyHolder<object>();
            other.Property = new object();
            // Exercise system
            var result = sut.Equals(other);
            // Verify outcome
            Assert.AreEqual(equalityResponse, result, "Equals");
            // Teardown
        }

        [TestMethod]
        public void EqualsOfIdenticalObjectsReturnsTrue()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            var other = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            // The rest of the test
            LikenessTest.CompareLikenessToObject(value, other, true);
        }

        [TestMethod]
        public void EqualsOfDifferentObjectsReturnFalse()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            var other = new QuadrupleParameterType<string, string, string, string>("amet", "consectetur", "adipisicing", "elit");
            // The rest of the test
            LikenessTest.CompareLikenessToObject(value, other, false);
        }

        [TestMethod]
        public void OmitAutoComparisonWillCauseDifferentObjectsToLookEqual()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            var other = new QuadrupleParameterType<string, string, string, string>("amet", "consectetur", "adipisicing", "elit");

            var sut = new Likeness<QuadrupleParameterType<string, string, string, string>, QuadrupleParameterType<string, string, string, string>>(value)
                .OmitAutoComparison();
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "OmitAutoComparison");
            // Teardown
        }

        [TestMethod]
        public void OmitAutoComparisonFollowedByWithDefaultEqualityWillOptInOnThosePropertiesOnlyAndReturnTrueWhenTheyMatch()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            var other = new QuadrupleParameterType<string, string, string, string>("Lorem", "ploeh", "dolor", "fnaah");

            var sut = new Likeness<QuadrupleParameterType<string, string, string, string>, QuadrupleParameterType<string, string, string, string>>(value)
                .OmitAutoComparison()
                .WithDefaultEquality(d => d.Parameter1)
                .WithDefaultEquality(d => d.Parameter3);
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "DefaultEquality");
            // Teardown
        }

        [TestMethod]
        public void OmitAutoComparisonFollowedByWithDefaultEqualityWillOptInOnThosePropertiesOnlyAndReturnFalseWhenTheyDoNotMatch()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            var other = new QuadrupleParameterType<string, string, string, string>("Lorem", "ploeh", "dolor", "fnaah");

            var sut = new Likeness<QuadrupleParameterType<string, string, string, string>, QuadrupleParameterType<string, string, string, string>>(value)
                .OmitAutoComparison()
                .WithDefaultEquality(d => d.Parameter1)
                .WithDefaultEquality(d => d.Parameter4);
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.IsFalse(result, "DefaultEquality");
            // Teardown
        }

        [TestMethod]
        public void OmitAutoComparisonFollowedByCorrectComboOfDefaultEqualityAndExplictyWithReturnsTrue()
        {
            // Fixture setup
            var value = new QuadrupleParameterType<string, string, string, string>("Lorem", "ipsum", "dolor", "sit");
            var other = new QuadrupleParameterType<string, string, string, string>("Lorem", "IPSUM", "dolor", "fnaah");

            var sut = new Likeness<QuadrupleParameterType<string, string, string, string>, QuadrupleParameterType<string, string, string, string>>(value)
                .OmitAutoComparison()
                .WithDefaultEquality(d => d.Parameter1)
                .With(d => d.Parameter2).EqualsWhen((s, d) => s.Parameter2.ToUpper() == d.Parameter2);
            // Exercise system
            bool result = sut.Equals(other);
            // Verify outcome
            Assert.IsTrue(result, "OmitAutoComparison...explicit With");
            // Teardown
        }

        private static void CompareLikenessToObject<TSource, TDestination>(TSource likenObject, TDestination comparee, bool expectedResult)
        {
            // Fixture setup
            var sut = new Likeness<TSource, TDestination>(likenObject);
            // Exercise system
            bool result = sut.Equals(comparee);
            // Verify outcome
            Assert.AreEqual<bool>(expectedResult, result, "If all public properties and fields are equal, Likeness should indicate equality.");
            // Teardown
        }

        public class A
        {
            public string X { get; set; }
        }

        public class B : A
        {
            public new int X { get; set; }
        }
    }
}
