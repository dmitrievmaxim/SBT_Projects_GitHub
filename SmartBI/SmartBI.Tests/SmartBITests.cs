using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace SmartBI.Tests
{
    [TestClass]
    public class SmartBITests
    {
        public TestContext TestContext { get; set; }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML","TestData.xml","User", DataAccessMethod.Sequential)]
        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine(Convert.ToString(TestContext.DataRow["Id"]));
        }
        
    }
}
