using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfRpt.Core.FunctionalTests.Models;
using PdfRpt.Core.Helper;

namespace PdfRpt.Core.FunctionalTests
{
    [TestClass]
    public class FastReflectionTests
    {
        [TestMethod]
        public void Test_TopLevel_Has_6_Items_Run1()
        {
            var obj = new TopLevel
            {
                Id = 1,
                Name = "Name 1",
                LastName = "Last Name 1",
                NestedType = new NestedType
                {
                    Key = "Key 1",
                    Value = 1
                }
            };

            var results = new DumpNestedProperties().DumpPropertyValues(obj);

            Assert.AreEqual(expected: 6, actual: results.Count);
            Assert.AreEqual(expected: "Last Name 1", actual: results.First(x => x.PropertyName == "LastName").PropertyValue);
            Assert.AreEqual(expected: "Key 1", actual: results.First(x => x.PropertyName == "NestedType.Key").PropertyValue);
        }

        [TestMethod]
        public void Test_TopLevel_Has_6_Items_Run2()
        {
            Test_TopLevel_Has_6_Items_Run1();
        }

        [TestMethod]
        public void Test_Employee_Has_6_Items_Run1()
        {
            var obj = new Employee
            {
                Id = 1,
                Name = "Name 1",
                Age = 23,
                Department = "Dep 1",
                Salary = 2300,
                WorkedHours = "08:00"
            };

            var results = new DumpNestedProperties().DumpPropertyValues(obj);

            Assert.AreEqual(expected: 6, actual: results.Count);
            Assert.AreEqual(expected: "Name 1", actual: results.First(x => x.PropertyName == "Name").PropertyValue);
        }

        [TestMethod]
        public void Test_Employee_Has_6_Items_Run2()
        {
            Test_Employee_Has_6_Items_Run1();
        }
    }
}