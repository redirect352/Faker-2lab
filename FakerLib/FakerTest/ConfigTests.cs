using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakerLib.Generator;
using FakerLib;
using System.Collections.Generic;

namespace FakerTest
{
    [TestClass]
    public class ConfigTest
    {

        private static FakerConfig config = new FakerConfig();
        private Faker faker = new Faker(config);


        [TestMethod]
        public void TestConfigForField()
        {
            config.Add<TestedClass, string, CityGenerator>(TestedClass => TestedClass.City);
            var testVal = faker.Create<TestedClass>();
            Assert.IsTrue(CityGenerator.IsGeneratedHere(testVal.City));
        }

        [TestMethod]
        public void TestConfigForProperty()
        {
            config.Add<TestedClass, string, CityGenerator>(TestedClass => TestedClass.CityProp);
            var testVal = faker.Create<TestedClass>();
            Assert.IsTrue(CityGenerator.IsGeneratedHere(testVal.CityProp));
        }

    }

    public class TestedClass
    {
        public string City;
        public string CityProp { get; set; }

    }
}
