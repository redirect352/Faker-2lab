using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FakerLib;
using System.Collections.Generic;

namespace FakerTest
{
    [TestClass]
    public class FakerTest
    {
        private Faker faker = new Faker();

        [TestMethod]
        public void TestValueType()
        {
            var testValue = faker.Create<int>();
            Assert.IsInstanceOfType(testValue, typeof(int));
        }

        [TestMethod]
        public void TestReferenceType()
        {
            var testValue = faker.Create<MySimpleClass>();

            Assert.IsInstanceOfType(testValue, typeof(MySimpleClass));
            Assert.IsNotNull(testValue.str);
        }

        [TestMethod]
        public void TestGenericType()
        {
            var testValue = faker.Create<List<int>>();

            Assert.IsInstanceOfType(testValue, typeof(List<int>));
            Assert.IsTrue(testValue.Count > 0);
        }

        [TestMethod]
        public void TestNestedClasses()
        {
            var testValue = faker.Create<MyClass>();
            Assert.IsNull(testValue.classMy);
            Assert.IsNull(testValue.Class2.classMy);
        }

        [TestMethod]
        public void TestConstructorPropertyInitialization()
        {
            var testValue = faker.Create<MyClass5>();
            Assert.AreNotEqual(testValue.Name, "defVal");
        }

        [TestMethod]
        public void TestNotSupportedValueType()
        {
            byte testValue = faker.Create<byte>();
            Assert.AreEqual(testValue,default(byte));
        }










    }


    public class MySimpleClass
    {
        public int count;
        public string str;
        public double Property1 { get; set; }

    }

    public class MyClass
    {
        public MyClass(int k, double par2, string ferr2)
        {
            field1 = k;
            field23 = par2;

        }

        public int field1 = 0;
        public double field23 = 0;
        public MyClass2 Class2;
        public MyClass classMy;

        public IList<int> ggg;

        public int Property1 { get; set; }
        public object PropertyObject { get; set; }
        private object PrivateProperty { get; set; }



    }

    public class MyClass2
    {
        public int count;
        public string str;
        public MyClass classMy { get; set; }

    }

    public class MyClass5
    {

        private string name = "defVal";
        public MyClass5(string s)
        {
            name = s;

        }

        public string Name
        {
            get { return name; }
        }
    }


}


